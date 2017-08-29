using System;
using System.IO;
using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;

namespace XSharp.Build.Tasks
{
    public class Xsc : ToolTask
    {
        [Required]
        public ITaskItem[] InputFiles { get; set; }

        public bool Append { get; set; } = false;

        public string OutputFile { get; set; } = null;

        protected override string ToolName => "xsc.exe";

        protected override string GenerateFullPathToTool()
        {
            if (String.IsNullOrWhiteSpace(ToolPath))
            {
                return Directory.GetCurrentDirectory();
            }

            return Path.GetFullPath(ToolPath);
        }

        protected override string GenerateCommandLineCommands()
        {
            var xBuilder = new CommandLineBuilder();

            if (ToolExe.Equals("dotnet", StringComparison.OrdinalIgnoreCase))
            {
                xBuilder.AppendSwitch("xsc.dll");
            }

            xBuilder.AppendFileNamesIfNotNull(InputFiles, " ");
            
            if (Append)
            {
                xBuilder.AppendSwitch("-Append");
            }

            if (OutputFile != null)
            {
                xBuilder.AppendSwitch($"-Out:\"${OutputFile}\"");
            }

            return xBuilder.ToString();
        }
    }
}
