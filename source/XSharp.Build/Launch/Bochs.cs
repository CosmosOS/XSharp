using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;

namespace XSharp.Build.Launch
{
    /// <summary>This class handles interactions with the Bochs emulation environment.</summary>
    public partial class Bochs : Host
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
        
        private string mDisplayLibrary;
        protected string DisplayLibrary
        {
            get
            {
                if (mDisplayLibrary == null)
                {
                    if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                    {
                        return "win32";
                    }
                    else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
                    {
                        // from Bochs docs:
                        // "gui_debug" - use GTK debugger gui (sdl, x) / Win32 debugger gui (sdl, sdl2, win32)
                        return "x";
                    }
                    else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
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

        /// <summary>
        /// Instantiation occurs when debugging engine is invoked to launch the process in suspended
        /// mode. Bochs process will eventually be launched later when debugging engine is instructed to
        /// Attach to the debugged process.
        /// </summary>
        public Bochs(bool aUseGDB, string aConfigurationFile, string aIsoFile, string aPipeServerName,
            bool aUseDebugVersion, bool aStartDebugGui, string aHardDisk = null, string aBochsDirectory = null,
            string aDisplayLibrary = null, DisplayLibraryOptions aDisplayLibraryOptions = DisplayLibraryOptions.None)
            : base(aUseGDB)
        {
            mBochsConfigurationFile = aConfigurationFile ?? throw new ArgumentNullException(nameof(aConfigurationFile));
            mIsoFile = aIsoFile ?? throw new ArgumentNullException(nameof(aIsoFile));
            mHardDiskFile = HardDiskHelpers.CreateDiskOnRequestedPathOrDefault(aHardDisk,
                Path.ChangeExtension(mIsoFile, ".vmdk"), HardDiskHelpers.HardDiskType.Vmdk);
            mDebugSymbolsPath = Path.ChangeExtension(mIsoFile, ".sym");

            mPipeServerName = aPipeServerName ?? throw new ArgumentNullException(nameof(aPipeServerName));

            mUseDebugVersion = aUseDebugVersion;

            mDisplayLibrary = aDisplayLibrary;
            mDisplayLibraryOptions = aDisplayLibraryOptions;

            if (String.IsNullOrWhiteSpace(aBochsDirectory) || !Directory.Exists(aBochsDirectory))
            {
                if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                {
                    mBochsDirectory = BochsSupport.BochsDirectory;
                }
                else
                {
                    throw new ArgumentException(aBochsDirectory);
                }
            }
            else
            {
                if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                {
                    mBochsExe = Path.Combine(mBochsDirectory, mUseDebugVersion ? "bochsdbg.exe" : "bochs.exe");
                }
                else
                {
                    // TODO - what's the extension of bochs exe on other platforms?
                    mBochsExe = Path.Combine(mBochsDirectory, mUseDebugVersion ? "bochsdbg" : "bochs");
                }
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

        public bool RedirectOutput = false;
        public Action<string> LogOutput;
        public Action<string> LogError;

        /// <summary>Initialize and start the Bochs process.</summary>
        public override void Start()
        {
            BochsSupport.ExtractBochsDebugSymbols(Path.ChangeExtension(mIsoFile, "map"), mDebugSymbolsPath);
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
            mBochsProcess.Exited += ExitCallback;
            mBochsProcess.Start();

            if (RedirectOutput)
            {
                mBochsProcess.BeginErrorReadLine();
                mBochsProcess.BeginOutputReadLine();
            }

            return;
        }

        private void ExitCallback(object sender, EventArgs e)
        {
            if (null != OnShutDown)
            {
                try
                {
                    OnShutDown(sender, e);
                }
                catch
                {
                }
            }
        }

        public override void Stop()
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
            CleanUp();
        }

        private void CleanUp()
        {
            OnShutDown(this, null);
            mBochsProcess.Exited -= ExitCallback;
            // TODO BlueSkeye : What kind of garbage may Bochs have left for us to clean ?
        }
    }
}
