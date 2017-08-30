using System;
using System.IO;
using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;

namespace XSharp.Build.Tasks
{
    // Cross Platform, without external tool: https://www.nuget.org/packages/DiscUtils.Iso9660
    // MSBuild doesn't support netstandard1.5 tasks, should we use multitargeting?
    public class MakeISO : ToolTask
    {
        [Required]
        public string InputFile { get; set; }

        [Required]
        public string OutputFile { get; set; }

        protected override string ToolName => "mkisofs.exe";

        protected override string GenerateFullPathToTool()
        {
            return Path.GetFullPath(ToolExe);
        }

        protected override bool ValidateParameters()
        {
            if (InputFile == null || !File.Exists(InputFile))
            {
                Log.LogError("InputFile is null or doesn't exist!");
                return false;
            }

            if (String.IsNullOrEmpty(OutputFile))
            {
                Log.LogError("OutputFile is null or empty!");
                return false;
            }

            try
            {
                Path.GetFullPath(OutputFile);
            }
            catch
            {
                Log.LogError("OutputFile is an invalid path!");
                return false;
            }

            return true;
        }

        protected override string GenerateCommandLineCommands()
        {
            var xBuilder = new CommandLineBuilder();
            var xIsoDirectory = Path.Combine(Path.GetDirectoryName(InputFile), "ISO");

            xBuilder.AppendSwitch("-relaxed-filenames");
            xBuilder.AppendSwitch("-J");
            xBuilder.AppendSwitch("-R");
            xBuilder.AppendSwitch("-o");
            xBuilder.AppendFileNameIfNotNull(OutputFile);
            xBuilder.AppendSwitch("-b isolinux.bin");
            xBuilder.AppendSwitch("-no-emul-boot");
            xBuilder.AppendSwitch("-boot-load-size 4");
            xBuilder.AppendSwitch("-boot-info-table");
            xBuilder.AppendFileNameIfNotNull(xIsoDirectory);

            return xBuilder.ToString();
        }
    }
}
