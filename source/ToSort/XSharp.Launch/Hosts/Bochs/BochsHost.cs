using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

namespace XSharp.Launch.Hosts.Bochs
{
    /// <summary>This class handles interactions with the Bochs emulation environment.</summary>
    public sealed class BochsHost : IHost, IDisposable
    {
        private const string BochsConfigurationFile = "Bochs.bxrc";

        private BochsLaunchSettings mLaunchSettings;

        private string mBochsExe;
        private Process mProcess;

        public event EventHandler ShutDown;

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
        public BochsHost(BochsLaunchSettings aLaunchSettings, bool aRedirectOutput = false,
            Action<string> aLogOutput = null, Action<string> aLogError = null)
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
                GenerateConfiguration();
            }
        }

        /// <summary>Initialize and start the Bochs process.</summary>
        public void Start()
        {
            //var xMapFile = Path.ChangeExtension(mLaunchSettings.IsoFile, ".map");
            //BochsSupport.TryExtractBochsDebugSymbols(xMapFile, BochsDebugSymbolsPath);

            mProcess = new Process();

            var xBochsStartInfo = mProcess.StartInfo;

            xBochsStartInfo.FileName = mBochsExe;

            var xExtraLog = "";
            if (mLaunchSettings.UseDebugVersion)
            {
                //xExtraLog = "-dbglog \"bochsdbg.log\"";
            }

            // Start Bochs without displaying the configuration interface (-q) and using the specified
            // configuration file (-f).

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
                mProcess.OutputDataReceived += (sender, args) => LogOutput(args.Data);
                mProcess.ErrorDataReceived += (sender, args) => LogError(args.Data);
            }
            // Register for process completion event so that we can funnel it to any code that
            // subscribed to this event in our base class.
            mProcess.EnableRaisingEvents = true;
            mProcess.Exited += delegate
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

                ShutDown?.Invoke(this, EventArgs.Empty);
            };

            mProcess.Start();

            if (RedirectOutput)
            {
                mProcess.BeginErrorReadLine();
                mProcess.BeginOutputReadLine();
            }
        }

        public void Kill()
        {
            try
            {
                mProcess?.Kill();
                mProcess?.WaitForExit();
            }
            catch (InvalidOperationException)
            {
            }
        }
        private void GenerateConfiguration()
        {
            var xConfiguration = GetDefaultConfiguration();

            var xRomImage = Path.Combine(mLaunchSettings.BochsDirectory, "BIOS-bochs-latest");
            var xVgaRomImage = Path.Combine(mLaunchSettings.BochsDirectory, "VGABIOS-lgpl-latest");

            var xVariables = new Dictionary<string, string>()
            {
                { "$CONFIG_INTERFACE$", ConfigInterface },
                { "$DISPLAY_LIBRARY$", DisplayLibrary },
                { "$DISPLAY_LIBRARY_OPTIONS$", mLaunchSettings.DisplayLibraryOptions.ToString() },
                { "$DEBUG_SYMBOLS_PATH$", BochsDebugSymbolsPath },
                { "$ROM_IMAGE$", xRomImage },
                { "$VGA_ROM_IMAGE$", xVgaRomImage },
                { "$CDROM_BOOT_PATH$", mLaunchSettings.IsoFile },
                { "$HARD_DISK_PATH$", mLaunchSettings.HardDiskFile },
                { "$PIPE_SERVER_NAME$", mLaunchSettings.PipeServerName }
            };

            xConfiguration = ReplaceConfigurationVariables(xConfiguration, xVariables);

            if (mLaunchSettings.UseDebugVersion)
            {
                xConfiguration = xConfiguration + "magic_break: enabled = 1" + Environment.NewLine;
            }

            using (var xWriter = File.CreateText(mLaunchSettings.ConfigurationFile))
            {
                xWriter.Write(xConfiguration);
            }
        }

        /// <summary>
        /// Reads the Bochs configuration file from assembly resources and returns it.
        /// </summary>
        /// <returns>Returns the Bochs configuration file.</returns>
        private string GetDefaultConfiguration()
        {
            using (var xStream = GetType().Assembly.GetManifestResourceStream(typeof(BochsHost), BochsConfigurationFile))
            {
                using (var xReader = new StreamReader(xStream))
                {
                    return xReader.ReadToEnd();
                }
            }
        }

        public void Dispose()
        {
            mProcess?.Dispose();
            GC.SuppressFinalize(this);
        }

        private static string ReplaceConfigurationVariables(string aConfiguration, Dictionary<string, string> aVariables)
        {
            foreach (var xVariable in aVariables)
            {
                if (xVariable.Key == null)
                {
                    throw new Exception();
                }

                aConfiguration = aConfiguration.Replace(xVariable.Key, xVariable.Value ?? String.Empty);
            }

            return aConfiguration;
        }
    }
}
