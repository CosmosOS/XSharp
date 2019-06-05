namespace XSharp.Launch.Hosts.VMware
{
    public class VMwareLaunchSettings
    {
        public string VMwareExecutable { get; set; }

        public string ConfigurationFile { get; set; }
        public bool OverwriteConfigurationFile { get; set; }

        public string IsoFile { get; set; }
        public string HardDiskFile { get; set; }

        public bool UseGDB { get; set; }

        public string PipeServerName { get; set; }
    }
}
