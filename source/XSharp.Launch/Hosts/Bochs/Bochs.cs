using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

namespace XSharp.Launch.Hosts.Bochs
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

        private BochsLaunchSettings mLaunchSettings;

        private string mBochsExe;
        private Process mBochsProcess;

        public string ConfigInterface
        {
            get
            {
                if (mLaunchSettings.ConfigurationInterface == null)
                {
                    switch (DisplayLibrary)
                    {
                        case "win32":
                            return "win32config";
                        case "wx":
                            return "wx";
                        default:
                            return "textconfig";
                    }
                }

                return mLaunchSettings.ConfigurationInterface;
            }
        }

        public string DisplayLibrary
        {
            get
            {
                if (mLaunchSettings.DisplayLibrary == null)
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

                return mLaunchSettings.DisplayLibrary;
            }
        }

        public string BochsDebugSymbolsPath => Path.ChangeExtension(mLaunchSettings.IsoFile, ".sym");

        public bool RedirectOutput { get; private set; }
        public Action<string> LogOutput { get; private set; }
        public Action<string> LogError { get; private set; }

        /// <summary>
        /// Instantiation occurs when debugging engine is invoked to launch the process in suspended
        /// mode. Bochs process will eventually be launched later when debugging engine is instructed to
        /// Attach to the debugged process.
        /// </summary>
#if true
        public Bochs(BochsLaunchSettings aLaunchSettings, bool aRedirectOutput = false,
            Action<string> aLogOutput = null, Action<string> aLogError = null)
#else
        public Bochs(bool aDebug, string aConfigurationFile, string aIsoFile, string aPipeServerName,
            bool aUseDebugVersion, bool aStartDebugGui, string aHardDisk = null, string aBochsDirectory = null,
            string aDisplayLibrary = null, DisplayLibraryOptions aDisplayLibraryOptions = DisplayLibraryOptions.None,
            bool aRedirectOutput = false, Action<string> aLogOutput = null, Action<string> aLogError = null)
#endif
        {
            mLaunchSettings = aLaunchSettings;

            RedirectOutput = aRedirectOutput;
            LogOutput = aLogOutput;
            LogError = aLogError;

            var xBochsDir = aLaunchSettings.BochsDirectory;

            if (String.IsNullOrWhiteSpace(xBochsDir) || !Directory.Exists(xBochsDir))
            {
                if (RuntimeHelper.IsWindows)
                {
                    aLaunchSettings.BochsDirectory = BochsSupport.BochsDirectory;
                }
            }

            if (RuntimeHelper.IsWindows)
            {
                mBochsExe = Path.Combine(
                    aLaunchSettings.BochsDirectory, aLaunchSettings.UseDebugVersion ? "bochsdbg.exe" : "bochs.exe");
            }
            else
            {
                // TODO - what's the extension of bochs exe on other platforms?
                mBochsExe = Path.Combine(
                    aLaunchSettings.BochsDirectory, aLaunchSettings.UseDebugVersion ? "bochsdbg" : "bochs");
            }

            if (mLaunchSettings.OverwriteConfigurationFile || !File.Exists(aLaunchSettings.ConfigurationFile))
            {
                GenerateConfiguration(aLaunchSettings.ConfigurationFile);
            }
        }

        /// <summary>Initialize and start the Bochs process.</summary>
        public void Start()
        {
            //var xMapFile = Path.ChangeExtension(mLaunchSettings.IsoFile, ".map");
            //BochsSupport.TryExtractBochsDebugSymbols(xMapFile, BochsDebugSymbolsPath);

            mBochsProcess = new Process();

            var xBochsStartInfo = mBochsProcess.StartInfo;

            xBochsStartInfo.FileName = mBochsExe;

            // Start Bochs without displaying the configuration interface (-q) and using the specified
            // configuration file (-f). The user is intended to edit the configuration file coming with
            // the Cosmos project whenever she wants to modify the environment.

            var xExtraLog = "";
            if (mLaunchSettings.UseDebugVersion)
            {
                //xExtraLog = "-dbglog \"bochsdbg.log\"";
            }

            xBochsStartInfo.Arguments = string.Format("-q {1} -f \"{0}\"", mLaunchSettings.ConfigurationFile, xExtraLog);
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
                var xLockFile = mLaunchSettings.HardDiskFile + ".lock";
                if (File.Exists(xLockFile))
                {
                    try
                    {
                        File.Delete(xLockFile);
                    }
                    catch (Exception ex)
                    {
                        throw new Exception($"The lock file couldn't be deleted! It has to be deleted manually. Lock file location: '{xLockFile}'.{Environment.NewLine}Exception:{Environment.NewLine}{ex.ToString()}");
                    }
                }
            };

            mBochsProcess.Start();

            if (RedirectOutput)
            {
                mBochsProcess.BeginErrorReadLine();
                mBochsProcess.BeginOutputReadLine();
            }
        }

        public void Stop()
        {
            try
            {
                mBochsProcess?.Kill();
                mBochsProcess?.WaitForExit();
            }
            catch
            {
            }
        }
    }
}
