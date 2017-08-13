using System;
using System.Collections.Generic;
using System.IO;

namespace XSharp.DotNetCLI {
  class Program {
    static void Main(string[] aArgs) {
      try {
        var xFiles = new List<string>();

        // Parse arguments
        var xCLI = new Build.CliProcessor();
        xCLI.Parse(aArgs);
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

        var xGen = new AsmGenerator();
        var xUserComments = xCLI.GetSwitch("UserComments");
        if (xUserComments != null) {
          xGen.EmitUserComments = xUserComments.Check("ON", new string[] { "ON", "OFF" }) == "ON";
        }
        var xSourceCode = xCLI.GetSwitch("SourceCode");
        if (xSourceCode != null) {
          xGen.EmitSourceCode = xSourceCode.Check("ON", new string[] { "ON", "OFF" }) == "ON";
        }

        var xPlugins = xCLI.GetSwitches("Plugin");
        foreach (var xPlugin in xPlugins) {
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
