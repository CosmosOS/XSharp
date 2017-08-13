using System;
using System.Collections.Generic;
using System.IO;
using XSharp.Assembler;

namespace XSharp.DotNetCLI {
  class Program {
    static void Main(string[] aArgs) {
      try {
        var xFiles = new List<string>();

        // Parse arguments
        var xCLI = new XSharp.Build.CliProcessor(aArgs);
        foreach (var xArg in xCLI.Items) {
          if (Directory.Exists(xArg.Value)) {
            string xPath = Path.GetFullPath(xArg.Value);
            xFiles.AddRange(Directory.GetFiles(xPath, "*.xs"));
          } else if (File.Exists(xArg.Value)) {
            xFiles.Add(Path.GetFullPath(xArg.Value));
          } else {
            throw new Exception("Not a valid file or directory: " + xArg);
          }
        }

        // Generate output
        var xGen = new AsmGenerator();
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
