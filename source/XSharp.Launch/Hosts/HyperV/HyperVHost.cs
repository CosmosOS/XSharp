using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Management.Automation;
using System.Management.Automation.Runspaces;
using System.Security.Principal;

namespace XSharp.Launch.Hosts.HyperV
{
    public sealed class HyperVHost : IHost, IDisposable
    {
        private HyperVLaunchSettings mLaunchSettings;

        private Process mProcess;
        
        private static bool IsProcessAdministrator => (new WindowsPrincipal(WindowsIdentity.GetCurrent())).IsInRole(WindowsBuiltInRole.Administrator);

        public event EventHandler ShutDown;

        public HyperVHost(HyperVLaunchSettings aLaunchSettings)
        {
            if (!RuntimeHelper.IsWindows)
            {
                throw new PlatformNotSupportedException();
            }

            mLaunchSettings = aLaunchSettings;
            
            if (!IsProcessAdministrator)
            {
                throw new Exception("Visual Studio must be run as administrator for Hyper-V to work");
            }
        }
        
        public void Start()
        {
            CreateVirtualMachine();

            // Target exe or file
            var info = new ProcessStartInfo(@"C:\Windows\sysnative\VmConnect.exe", @"""localhost"" ""Cosmos""")
            {
                UseShellExecute = false
            };

            mProcess = new Process();
            mProcess.StartInfo = info;
            mProcess.EnableRaisingEvents = true;

            mProcess.Exited += delegate
            {
                ShutDown?.Invoke(this, EventArgs.Empty);
            };

            mProcess.Start();

            RunPowershellScript("Start-VM -Name Cosmos");
        }
        
        public void Kill()
        {
            RunPowershellScript("Stop-VM -Name Cosmos -TurnOff -ErrorAction Ignore");
            mProcess.Kill();
        }
        
        private void CreateVirtualMachine()
        {
            RunPowershellScript("Stop-VM -Name Cosmos -TurnOff -ErrorAction Ignore");

            RunPowershellScript("Remove-VM -Name Cosmos -Force -ErrorAction Ignore");
            RunPowershellScript("New-VM -Name Cosmos -MemoryStartupBytes 268435456 -BootDevice CD");

            RunPowershellScript($@"Add-VMHardDiskDrive -VMName Cosmos -ControllerNumber 0 -ControllerLocation 0 -Path ""{mLaunchSettings.HardDiskFile}""");
            RunPowershellScript($@"Set-VMDvdDrive -VMName Cosmos -ControllerNumber 1 -ControllerLocation 0 -Path ""{mLaunchSettings.IsoFile}""");
            RunPowershellScript(@"Set-VMComPort -VMName Cosmos -Path \\.\pipe\CosmosSerial -Number 1");
        }

        public void Dispose()
        {
            mProcess?.Dispose();
            //GC.SuppressFinalize(this);
        }

        private static void RunPowershellScript(string text)
        {
            using (Runspace runspace = RunspaceFactory.CreateRunspace())
            {
                runspace.Open();

                Pipeline pipeline = runspace.CreatePipeline();

                pipeline.Commands.AddScript(text);
                pipeline.Commands.Add("Out-String");

                Collection<PSObject> results = pipeline.Invoke();
                foreach (PSObject obj in results)
                {
                    Debug.WriteLine(obj.ToString());
                }
            }
        }
    }
}
