using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using Microsoft.Win32;

namespace XSharp.Launch.Hosts.VMware
{
    public enum VMwareEdition
    {
        Workstation,
        Player
    }

    public class VMware : IHost
    {
        private const string VMwareConfigurationFile = "VMware.vmx";

        private VMwareLaunchSettings mLaunchSettings;

        private Process mProcess;

        public event EventHandler ShutDown;

        public VMware(VMwareLaunchSettings aLaunchSettings)
        {
            if (!(RuntimeHelper.IsWindows || RuntimeHelper.IsLinux))
            {
                throw new PlatformNotSupportedException();
            }

            mLaunchSettings = aLaunchSettings;

            var xVMwareExecutable = mLaunchSettings.VMwareExecutable;

            if (String.IsNullOrEmpty(xVMwareExecutable) || !File.Exists(xVMwareExecutable))
            {
                if (RuntimeHelper.IsWindows)
                {
                    mLaunchSettings.VMwareExecutable = GetPathname("VMware Workstation", "vmware.exe")
                        ?? GetPathname("VMware Player", "vmplayer.exe");
                }
            }
        }

        protected string GetPathname(string aKey, string aExe)
        {
            using (var xRegKey = RegistryKey.OpenBaseKey(
                RegistryHive.LocalMachine, RegistryView.Registry32).OpenSubKey(@"Software\VMware, Inc.\" + aKey, false))
            {
                if (xRegKey != null)
                {
                    var xInstallPath = (string)xRegKey.GetValue("InstallPath");

                    if (xInstallPath == null)
                    {
                        return null;
                    }

                    string xResult = Path.Combine(xInstallPath, aExe);

                    if (File.Exists(xResult))
                    {
                        return xResult;
                    }
                }

                return null;
            }
        }

        public void Start()
        {
            if (mLaunchSettings.OverwriteConfigurationFile || !File.Exists(mLaunchSettings.ConfigurationFile))
            {
                Cleanup();
                CreateVmx();
            }

            // Target exe or file
            mProcess = new Process();

            var xVMwareStartInfo = mProcess.StartInfo;

            xVMwareStartInfo.FileName = mLaunchSettings.VMwareExecutable;

            string xVmxPath = "\"" + mLaunchSettings.ConfigurationFile + "\"";
            //if (mEdition == VMwareEdition.Player)
            {
                xVMwareStartInfo.Arguments = xVmxPath;
            }
            //else
            //{
            //    // -x: Auto power on VM. Must be small x, big X means something else.
            //    // -q: Close VMware when VM is powered off.
            //    // Options must come beore the vmx, and cannot use shellexecute
            //    xPSI.Arguments = "-x -q " + xVmxPath;
            //}
            xVMwareStartInfo.UseShellExecute = false;  //must be true to allow elevate the process, sometimes needed if vmware only runs with admin rights
            mProcess.EnableRaisingEvents = true;

            mProcess.Exited += delegate
            {
                ShutDown?.Invoke(this, EventArgs.Empty);
            };

            mProcess.Start();
        }

        public void Stop()
        {
            try
            {
                mProcess?.Kill();
                mProcess?.WaitForExit();
            }
            catch (InvalidOperationException)
            {
            }
            finally
            {
                Cleanup();
            }
        }

        protected void DeleteFiles(string aPath, string aPattern)
        {
            var xFiles = Directory.GetFiles(aPath, aPattern);
            foreach (var xFile in xFiles)
            {
                File.Delete(xFile);
            }
        }

        protected void Cleanup()
        {
            try
            {
                var xVmxFile = mLaunchSettings.ConfigurationFile;

                string xPath = Path.GetDirectoryName(xVmxFile);
                // Delete old Cosmos.vmx and other files that might be left over from previous run.
                // Especially important with newer versions of VMware player which defaults to suspend
                // when the close button is used.
                File.Delete(xVmxFile);
                File.Delete(Path.ChangeExtension(xVmxFile, ".nvram"));
                // Delete the auto snapshots that latest vmware players create as default.
                // It creates them with suffixes though, so we need to wild card find them.
                DeleteFiles(xPath, "*.vmxf");
                DeleteFiles(xPath, "*.vmss");
                DeleteFiles(xPath, "*.vmsd");
                DeleteFiles(xPath, "*.vmem");
                // Delete log files so that logged data is only from last boot
                File.Delete(Path.Combine(xPath, "vmware.log"));
                File.Delete(Path.Combine(xPath, "vmware-0.log"));
                File.Delete(Path.Combine(xPath, "vmware-1.log"));
                File.Delete(Path.Combine(xPath, "vmware-2.log"));
            }
            catch (Exception)
            {
                // Ignore errors, users can stop VS while VMware is still running and files will be locked.
            }
        }

        private void CreateVmx()
        {
            //var xNvramFile = Path.ChangeExtension(mLaunchSettings.ConfigurationFile, ".nvram");

            //if (!File.Exists(xNvramFile))
            //{
            //    using (var xStream = Assembly.GetExecutingAssembly().GetManifestResourceStream(typeof(VMware), "VMware.nvram"))
            //    {
            //        using (var xFile = File.Create(xNvramFile))
            //        {
            //            xStream.CopyTo(xFile);
            //        }
            //    }
            //}

            var xConfiguration = GetDefaultConfiguration();

            var xVariables = new Dictionary<string, string>()
            {
                { "$NVRAM_PATH$", Path.ChangeExtension(Path.GetFileName(mLaunchSettings.ConfigurationFile), ".nvram") },
                { "$ISO_PATH$", mLaunchSettings.IsoFile },
                { "$HARD_DISK_PATH$", mLaunchSettings.HardDiskFile },
                { "$PIPE_SERVER_NAME$", mLaunchSettings.PipeServerName }
            };

            xConfiguration = ReplaceConfigurationVariables(xConfiguration, xVariables);

            if (mLaunchSettings.UseGDB)
            {
                xConfiguration += Environment.NewLine;
                xConfiguration += "debugStub.hideBreakpoints = \"TRUE\"" + Environment.NewLine;
                xConfiguration += "debugStub.listen.guest32 = \"TRUE\"" + Environment.NewLine;
                xConfiguration += "debugStub.listen.guest32.remote = \"TRUE\"" + Environment.NewLine;
                xConfiguration += "monitor.debugOnStartGuest32 = \"TRUE\"" + Environment.NewLine;
            }

            var xConfigurationFile = mLaunchSettings.ConfigurationFile;

            using (var xSrc = new StreamReader(Assembly.GetExecutingAssembly().GetManifestResourceStream(typeof(VMware), "VMware.vmx")))
            {
                using (var xDest = new StreamWriter(File.Open(xConfigurationFile, FileMode.Create)))
                {
                    string xLine;
                    while ((xLine = xSrc.ReadLine()) != null)
                    {
                        var xParts = xLine.Split('=');
                        if (xParts.Length == 2)
                        {
                            string xName = xParts[0].Trim();
                            string xValue = xParts[1].Trim();

                            if ((xName == "uuid.location") || (xName == "uuid.bios"))
                            {
                                // We delete uuid entries so VMware doesnt ask the user "Did you move or copy" the file
                                xValue = null;

                            }
                            else if (xName == "ide1:0.fileName")
                            {
                                // Set the ISO file for booting
                                xValue = "\"" + mLaunchSettings.IsoFile + "\"";
                            }
                            else if (xName == "ide0:0.fileName")
                            {
                                xValue = "\"" + mLaunchSettings.HardDiskFile + "\"";
                            }
                            else if (xName == "nvram")
                            {
                                // Point it to an initially non-existent nvram.
                                // This has the effect of disabling PXE so the boot is faster.
                                xValue = "\"" + Path.ChangeExtension(xConfigurationFile, ".nvram") + "\"";
                            }

                            if (xValue != null)
                            {
                                xDest.WriteLine(xName + " = " + xValue);
                            }
                        }
                    }

                    if (mLaunchSettings.UseGDB)
                    {
                        xDest.WriteLine();
                        xDest.WriteLine("debugStub.listen.guest32 = \"TRUE\"");
                        xDest.WriteLine("debugStub.hideBreakpoints = \"TRUE\"");
                        xDest.WriteLine("monitor.debugOnStartGuest32 = \"TRUE\"");
                        xDest.WriteLine("debugStub.listen.guest32.remote = \"TRUE\"");
                    }
                }
            }
        }

        private string GetDefaultConfiguration()
        {
            using (var xStream = GetType().Assembly.GetManifestResourceStream(typeof(VMware), VMwareConfigurationFile))
            {
                using (var xReader = new StreamReader(xStream))
                {
                    return xReader.ReadToEnd();
                }
            }
        }

        private string ReplaceConfigurationVariables(string aConfiguration, Dictionary<string, string> aVariables)
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
