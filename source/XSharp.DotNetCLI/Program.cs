using System;
using System.Collections.Generic;
using System.IO;
using XSharp.Assembler;

namespace XSharp.DotNetCLI {
  class Program {
    static void Main(string[] aArgs) {
      try {
        var xCLI = new XSharp.Build.CliProcessor(aArgs);
        var xGen = new AsmGenerator();
        var xFiles = new List<string>();

        // Parse arguments
        foreach (var xArg in aArgs) {
          if (xArg.StartsWith("-")) {
            string xOpt = xArg.Substring(1).ToUpper();
            throw new Exception("No matching switch found: " + xArg);
          } else {
            if (Directory.Exists(xArg)) {
              string xPath = Path.GetFullPath(xArg);
              xFiles.AddRange(Directory.GetFiles(xPath, "*.xs"));
            } else if (File.Exists(xArg)) {
              xFiles.Add(Path.GetFullPath(xArg));
            } else {
              throw new Exception("Not a valid file or directory: " + xArg);
            }
          }
        }

        // Generate output
        foreach (var xFile in xFiles) {
          Console.WriteLine(xFile);
          xGen.GenerateToFiles(xFile);
        }

        // Finalize
        Console.WriteLine("Done.");
      } catch (Exception ex) {
        Console.WriteLine(ex.ToString());
        System.Threading.Thread.Sleep(3000);
        Environment.Exit(1);
      }
    }
  }
}
