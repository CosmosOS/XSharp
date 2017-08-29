using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Management.Automation;
using System.Management.Automation.Runspaces;
using System.Runtime.InteropServices;
using System.Security.Principal;

namespace XSharp.Build.Launch
{
    public class HyperV : Host
    {
        protected string mIsoFile;
        protected string mHardDiskFile;
        protected Process mProcess;
        
        private static bool IsProcessAdministrator => (new WindowsPrincipal(WindowsIdentity.GetCurrent())).IsInRole(WindowsBuiltInRole.Administrator);
        
        public HyperV(bool aUseGDB, string aIsoFile, string aHardDisk = null) : base(aUseGDB)
        {
            if (!RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                throw new PlatformNotSupportedException();
            }

            mIsoFile = aIsoFile ?? throw new ArgumentNullException(nameof(aIsoFile));

            if (!IsProcessAdministrator)
            {
                throw new Exception("Visual Studio must be run as administrator for Hyper-V to work");
            }

            mHardDiskFile = HardDiskHelpers.CreateDiskOnRequestedPathOrDefault(aHardDisk,
                Path.ChangeExtension(mIsoFile, ".vhdx"), HardDiskHelpers.HardDiskType.Vhdx);
        }
        
        public override void Start()
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
            mProcess.Exited += (Object aSender, EventArgs e) =>
            {
                OnShutDown?.Invoke(aSender, e);
            };
            mProcess.Start();

            RunPowershellScript("Start-VM -Name Cosmos");
        }
        
        public override void Stop()
        {
            RunPowershellScript("Stop-VM -Name Cosmos -TurnOff -ErrorAction Ignore");
            mProcess.Kill();
        }
        
        protected void CreateVirtualMachine()
        {
            RunPowershellScript("Stop-VM -Name Cosmos -TurnOff -ErrorAction Ignore");

            RunPowershellScript("Remove-VM -Name Cosmos -Force -ErrorAction Ignore");
            RunPowershellScript("New-VM -Name Cosmos -MemoryStartupBytes 268435456 -BootDevice CD");
            if (!File.Exists(mHardDiskFile))
            {
                RunPowershellScript($@"New-VHD -SizeBytes 268435456 -Dynamic -Path ""{mHardDiskFile}""");
            }

            RunPowershellScript($@"Add-VMHardDiskDrive -VMName Cosmos -ControllerNumber 0 -ControllerLocation 0 -Path ""{mHardDiskFile}""");
            RunPowershellScript($@"Set-VMDvdDrive -VMName Cosmos -ControllerNumber 1 -ControllerLocation 0 -Path ""{mIsoFile}""");
            RunPowershellScript(@"Set-VMComPort -VMName Cosmos -Path \\.\pipe\CosmosSerial -Number 1");
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
                    System.Diagnostics.Debug.WriteLine(obj.ToString());
                }
            }
        }
    }
}
