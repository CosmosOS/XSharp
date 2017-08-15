using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;

namespace XSharp.Build.Tasks
{
    public class Xsc : Task
    {
        [Required]
        public ITaskItem[] XSharpFiles { get; set; }

        public bool Append { get; set; } = false;

        public string OutputFile { get; set; } = null;

        public override bool Execute()
        {
            try
            {
                var xArgs = new List<string>();

                foreach (var xFile in XSharpFiles)
                {
                    xArgs.Add($"\"{xFile.GetMetadata("FullPath")}\"");
                }

                if (Append)
                {
                    xArgs.Add("-Append");
                }

                if (OutputFile != null && !String.IsNullOrWhiteSpace(OutputFile))
                {
                    OutputFile = Path.GetFullPath(OutputFile);
                    xArgs.Add($"-Out:\"{OutputFile}\"");
                }

                xArgs.Insert(0, "xsharp");

                var xPSI = new ProcessStartInfo
                {
                    FileName = "dotnet",
                    Arguments = String.Join(" ", xArgs),
                    RedirectStandardOutput = true,
                    RedirectStandardError = true
                };

                var xProcess = new Process
                {
                    StartInfo = xPSI
                };

                Log.LogMessagesFromStream(xProcess.StandardOutput, MessageImportance.Low);

                xProcess.ErrorDataReceived += delegate (object sender, DataReceivedEventArgs e)
                {
                    Log.LogError(e.Data);
                };

                xProcess.Start();
                xProcess.WaitForExit();

                return true;
            }
            catch (Exception e)
            {
                Log.LogErrorFromException(e);
                return false;
            }
        }

        private void XProcess_ErrorDataReceived(object sender, DataReceivedEventArgs e)
        {
            throw new NotImplementedException();
        }
    }
}
