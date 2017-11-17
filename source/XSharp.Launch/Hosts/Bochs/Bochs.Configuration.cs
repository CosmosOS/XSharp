using System;
using System.Collections.Generic;
using System.IO;

namespace XSharp.Launch.Hosts.Bochs
{
    public partial class Bochs
    {
        private const string BochsConfigurationFile = "Bochs.bxrc";

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
            using (var xStream = GetType().Assembly.GetManifestResourceStream(typeof(Bochs), BochsConfigurationFile))
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
