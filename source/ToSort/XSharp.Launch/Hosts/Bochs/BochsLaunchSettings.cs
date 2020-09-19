namespace XSharp.Launch.Hosts.Bochs
{
    public class BochsLaunchSettings
    {
        public BochsLaunchSettings()
        {
            DisplayLibraryOptions = new BochsDisplayLibraryOptions();
        }

        public string BochsDirectory { get; set; }

        public string ConfigurationFile { get; set; }
        public bool OverwriteConfigurationFile { get; set; }

        public string IsoFile { get; set; }
        public string HardDiskFile { get; set; }

        public string ConfigurationInterface { get; set; }

        public string DisplayLibrary { get; set; }
        public BochsDisplayLibraryOptions DisplayLibraryOptions { get; set; }

        public bool UseDebugVersion { get; set; }
        public bool StartDebugGUI { get; set; }

        public string PipeServerName { get; set; }
    }

    public class BochsDisplayLibraryOptions
    {
        public bool GUIDebug { get; set; }
        public bool HideIPS { get; set; }
        public bool NoKeyRepeat { get; set; }

        public override string ToString()
        {
            return $"{(GUIDebug ? "gui_debug " : "")}{(HideIPS ? "hideIPS " : "")}{(NoKeyRepeat ? "nokeyrepeat" : "")}";
        }
    }
}
