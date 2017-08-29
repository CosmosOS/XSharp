using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;

namespace XSharp.Build.Launch
{
    public partial class Bochs
    {
        private Dictionary<string, string> defaultConfigs = new Dictionary<string, string>();

        private string mDefaultConfiguration;
        protected string DefaultConfiguration
        {
            get
            {
                if (mDefaultConfiguration == null)
                {
                    mDefaultConfiguration = GetDefaultConfiguration();
                }

                return mDefaultConfiguration;
            }
        }
        
        private void GenerateConfiguration(string aFilePath)
        {
            var xConfiguration = DefaultConfiguration;

            xConfiguration = xConfiguration.Replace("$CONFIG_INTERFACE$", ConfigInterface);
            xConfiguration = xConfiguration.Replace("$DISPLAY_LIBRARY$", DisplayLibrary);
            xConfiguration = xConfiguration.Replace("$DISPLAY_LIBRARY_OPTIONS$", GetDisplayLibraryOptionsAsString());
            xConfiguration = xConfiguration.Replace("$DEBUG_SYMBOLS_PATH$", mDebugSymbolsPath);
            xConfiguration = xConfiguration.Replace("$ROM_IMAGE$", Path.Combine(mBochsDirectory, "BIOS-bochs-latest"));
            xConfiguration = xConfiguration.Replace("$VGA_ROM_IMAGE$", Path.Combine(mBochsDirectory, "VGABIOS-lgpl-latest"));
            xConfiguration = xConfiguration.Replace("$CDROM_BOOT_PATH", mIsoFile);
            xConfiguration = xConfiguration.Replace("$HARD_DISK_PATH", mHardDiskFile);
            xConfiguration = xConfiguration.Replace("$PIPE_SERVER_NAME", mPipeServerName);

            using (var xStream = File.Create(aFilePath))
            {
                using (var xWriter = new StreamWriter(xStream))
                {
                    xWriter.WriteAsync(xConfiguration);
                }
            }
        }

        /// <summary>
        /// Reads the Bochs configuration file from manifest and returns it.
        /// </summary>
        /// <returns>Returns the Bochs configuration file.</returns>
        private string GetDefaultConfiguration()
        {
            var xConfiguration = "";

            using (var xStream = GetType().Assembly.GetManifestResourceStream("XSharp.Build.Resources.Cosmos.bxrc"))
            {
                using (var xReader = new StreamReader(xStream))
                {
                    xConfiguration = xReader.ReadToEnd();
                }
            }

            return xConfiguration;
        }

        private string GetDisplayLibraryOptionsAsString()
        {
            return mDisplayLibraryOptions.ToString().ToLower().Replace("guidebug", "gui_debug");
        }
    }
}
