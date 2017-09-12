using System;
using System.IO;
using System.Linq;
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

        protected override bool ValidateParameters()
        {
            if (InputFiles.Length == 0)
            {
                Log.LogError("No input files specified!");
                return false;
            }

            bool xInvalidFile = false;

            foreach (var xFile in InputFiles)
            {
                var xFullPath = xFile.GetMetadata("FullPath");

                if (String.IsNullOrWhiteSpace(xFullPath))
                {
                    Log.LogError($"Input file is empty! Input files: '${String.Join(";", InputFiles.Select(f => f.GetMetadata("Identity")))}'");
                }
                else if (!File.Exists(xFullPath))
                {
                    Log.LogError($"Input file '${xFullPath}' doesn't exist!");
                }
            }

            if (xInvalidFile)
            {
                return false;
            }

            if (String.IsNullOrEmpty(OutputFile))
            {
                Log.LogError("No output file specified!");
                return false;
            }

            return true;
        }

        protected override string GenerateFullPathToTool()
        {
            if (String.IsNullOrWhiteSpace(ToolExe))
            {
                return null;
            }

            if (String.IsNullOrWhiteSpace(ToolPath))
            {
                if (ToolExe.Equals("dotnet", StringComparison.OrdinalIgnoreCase))
                {
                    return ToolExe;
                }

                return Path.Combine(Directory.GetCurrentDirectory(), ToolExe);
            }

            return Path.Combine(Path.GetFullPath(ToolPath), ToolExe);
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
