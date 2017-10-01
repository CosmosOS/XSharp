using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;

namespace XSharp.Launch
{
    /// <summary>This class handles interactions with the Bochs emulation environment.</summary>
    public partial class Bochs : IHost
    {
        [Flags]
        public enum DisplayLibraryOptions
        {
            None = 0b0,
            /// <summary>
            /// Start Bochs debugger GUI.
            /// <para>
            /// Supported display libraries:
            /// GTK debugger GUI (sdl, x);
            /// Win32 debugger GUI (sdl, sdl2, win32)
            /// </para>
            /// </summary>
            GUIDebug = 0b01,
            /// <summary>
            /// Disable IPS output in status bar.
            /// <para>
            /// Supported display libraries: rfb, sdl, sdl2, vncsrv, win32, wx, x
            /// </para>
            /// </summary>
            HideIPS = 0b10,
            /// <summary>
            /// Turn off host keyboard repeat.
            /// <para>
            /// Supported display libraries: sdl, sdl2, win32, x
            /// </para>
            /// </summary>
            NoKeyRepeat = 0b100,
            /// <summary>
            /// Time (in seconds) to wait for client.
            /// <para>
            /// Supported display libraries: rfb, vncsrv
            /// </para>
            /// </summary>
            Timeout = 0b1000
        }

        private Process mBochsProcess;

        private string mBochsDirectory;
        private string mBochsExe;

        private string mBochsConfigurationFile;
        private string mIsoFile;
        private string mHardDiskFile;
        private string mDebugSymbolsPath;

        private string mPipeServerName;

        private bool mUseDebugVersion;

        private string mConfigInterface;
        public string ConfigInterface
        {
            get
            {
                if (mConfigInterface == null)
                {
                    switch (DisplayLibrary)
                    {
                        case "win32":
                            mConfigInterface = "win32config";
                            break;
                        case "wx":
                            mConfigInterface = "wx";
                            break;
                        default:
                            mConfigInterface = "textconfig";
                            break;
                    }
                }

                return mConfigInterface;
            }
        }

        private string mDisplayLibrary;
        public string DisplayLibrary
        {
            get
            {
                if (mDisplayLibrary == null)
                {
                    if (RuntimeHelper.IsWindows)
                    {
                        return "win32";
                    }
                    else if (RuntimeHelper.IsOSX)
                    {
                        // from Bochs docs:
                        // "gui_debug" - use GTK debugger gui (sdl, x) / Win32 debugger gui (sdl, sdl2, win32)
                        return "x";
                    }
                    else if (RuntimeHelper.IsLinux)
                    {
                        return "x";
                    }
                    else
                    {
                        throw new NotSupportedException();
                    }
                }

                return mDisplayLibrary;
            }
        }

        private DisplayLibraryOptions mDisplayLibraryOptions;
        
        public bool RedirectOutput { get; private set; }
        public Action<string> LogOutput { get; private set; }
        public Action<string> LogError { get; private set; }

        /// <summary>
        /// Instantiation occurs when debugging engine is invoked to launch the process in suspended
        /// mode. Bochs process will eventually be launched later when debugging engine is instructed to
        /// Attach to the debugged process.
        /// </summary>
        public Bochs(bool aDebug, string aConfigurationFile, string aIsoFile, string aPipeServerName,
            bool aUseDebugVersion, bool aStartDebugGui, string aHardDisk = null, string aBochsDirectory = null,
            string aDisplayLibrary = null, DisplayLibraryOptions aDisplayLibraryOptions = DisplayLibraryOptions.None,
            bool aRedirectOutput = false, Action<string> aLogOutput = null, Action<string> aLogError = null)
        {
            mBochsConfigurationFile = aConfigurationFile ?? throw new ArgumentNullException(nameof(aConfigurationFile));
            mIsoFile = aIsoFile ?? throw new ArgumentNullException(nameof(aIsoFile));
            mHardDiskFile = HardDiskHelpers.CreateDiskOnRequestedPathOrDefault(aHardDisk,
                Path.ChangeExtension(mIsoFile, ".vmdk"), HardDiskHelpers.HardDiskType.Vmdk);
            mDebugSymbolsPath = Path.ChangeExtension(mIsoFile, ".sym");

            mPipeServerName = aPipeServerName ?? (aDebug ? throw new ArgumentNullException(nameof(aPipeServerName)) : String.Empty) ;

            mUseDebugVersion = aUseDebugVersion;

            mDisplayLibrary = aDisplayLibrary;
            mDisplayLibraryOptions = aDisplayLibraryOptions;

            RedirectOutput = aRedirectOutput;
            LogOutput = aLogOutput;
            LogError = aLogError;

            if (String.IsNullOrWhiteSpace(aBochsDirectory) || !Directory.Exists(aBochsDirectory))
            {
                if (RuntimeHelper.IsWindows)
                {
                    mBochsDirectory = BochsSupport.BochsDirectory;
                }
                else
                {
                    throw new ArgumentException(aBochsDirectory);
                }
            }

            if (RuntimeHelper.IsWindows)
            {
                mBochsExe = Path.Combine(mBochsDirectory, mUseDebugVersion ? "bochsdbg.exe" : "bochs.exe");
            }
            else
            {
                // TODO - what's the extension of bochs exe on other platforms?
                mBochsExe = Path.Combine(mBochsDirectory, mUseDebugVersion ? "bochsdbg" : "bochs");
            }
            
            GenerateConfiguration(mBochsConfigurationFile);
        }


        /// <summary>
        /// Fix the content of the configuration file, replacing each of the symbolic variable occurence
        /// with its associated value.
        /// </summary>
        /// <param name="symbols">A set of key/value pairs where the key is the name of a variable. The value is
        /// used for variable replacement. Variables are case sensistive.</param>
        internal void FixBochsConfiguration(KeyValuePair<string, string>[] symbols)
        {
            if ((null == symbols) || (0 == symbols.Length))
            {
                return;
            }
            string content;
            using (StreamReader reader = new StreamReader(File.Open(mBochsConfigurationFile, FileMode.Open, FileAccess.Read)))
            {
                content = reader.ReadToEnd();
            }
            foreach (KeyValuePair<string, string> pair in symbols)
            {
                string variableName = string.Format("$({0})", pair.Key);

                content.Replace(variableName, pair.Value);
            }
            using (StreamWriter writer = new StreamWriter(File.Open(mBochsConfigurationFile, FileMode.Create, FileAccess.Write)))
            {
                writer.Write(content);
            }
        }

        /// <summary>Initialize and start the Bochs process.</summary>
        public void Start()
        {
            BochsSupport.TryExtractBochsDebugSymbols(Path.ChangeExtension(mIsoFile, "map"), mDebugSymbolsPath);
            mBochsProcess = new Process();
            ProcessStartInfo xBochsStartInfo = mBochsProcess.StartInfo;
            xBochsStartInfo.FileName = mBochsExe;

            // Start Bochs without displaying the configuration interface (-q) and using the specified
            // configuration file (-f). The user is intended to edit the configuration file coming with
            // the Cosmos project whenever she wants to modify the environment.

            var xExtraLog = "";
            if (mUseDebugVersion)
            {
                //xExtraLog = "-dbglog \"bochsdbg.log\"";
            }

            xBochsStartInfo.Arguments = string.Format("-q {1} -f \"{0}\"", mBochsConfigurationFile, xExtraLog);
            xBochsStartInfo.WorkingDirectory = Path.GetDirectoryName(mIsoFile);
            xBochsStartInfo.UseShellExecute = true;

            if (RedirectOutput)
            {
                if (LogOutput == null)
                {
                    throw new Exception("No LogOutput handler specified!");
                }
                if (LogError == null)
                {
                    throw new Exception("No LogError handler specified!");
                }

                xBochsStartInfo.RedirectStandardOutput = true;
                xBochsStartInfo.RedirectStandardError = true;
                mBochsProcess.OutputDataReceived += (sender, args) => LogOutput(args.Data);
                mBochsProcess.ErrorDataReceived += (sender, args) => LogError(args.Data);
            }
            // Register for process completion event so that we can funnel it to any code that
            // subscribed to this event in our base class.
            mBochsProcess.EnableRaisingEvents = true;
            mBochsProcess.Exited += delegate
            {
                var xLockFile = mHardDiskFile + ".lock";
                if (File.Exists(xLockFile))
                {
                    try
                    {
                        File.Delete(xLockFile);
                    }
                    catch (Exception ex)
                    {
                        throw new Exception($"The lock file couldn't be deleted! You have to delete it manually. Lock file location: '{xLockFile}'.{Environment.NewLine}Exception:{Environment.NewLine}{ex.ToString()}")
                    }
            };

            mBochsProcess.Start();

            if (RedirectOutput)
            {
                mBochsProcess.BeginErrorReadLine();
                mBochsProcess.BeginOutputReadLine();
            }

            return;
        }

        public void Stop()
        {
            if (null != mBochsProcess)
            {
                try
                {
                    mBochsProcess.Kill();
                }
                catch
                {
                }
            }
        }
    }
}
