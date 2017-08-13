using System;
using System.Collections.Generic;
using System.IO;

namespace XSharp.DotNetCLI {
  class Program {
    static void Main(string[] aArgs) {
      try {
        var xGen = new AsmGenerator();

        // Parse arguments
        var xCLI = new Build.CliProcessor();
        xCLI.Parse(aArgs);

        // Switches
        var xUserComments = xCLI.GetSwitch("UserComments");
        if (xUserComments != null) {
          xGen.EmitUserComments = xUserComments.Check("ON", new string[] { "ON", "OFF" }) == "ON";
        }
        //
        var xSourceCode = xCLI.GetSwitch("SourceCode");
        if (xSourceCode != null) {
          xGen.EmitSourceCode = xSourceCode.Check("ON", new string[] { "ON", "OFF" }) == "ON";
        }
        //
        var xPlugins = xCLI.GetSwitches("Plugin");
        foreach (var xPlugin in xPlugins) {
        }

        // List of files
        var xFiles = new List<string>();
        foreach (var xArg in xCLI.Items) {
          string xVal = xArg.Value;

          if (Directory.Exists(xVal)) {
            // If dir specified, find all .xs files
            string xPath = Path.GetFullPath(xVal);
            xFiles.AddRange(Directory.GetFiles(xPath, "*.xs"));

          } else if (File.Exists(xVal)) {
            string xExt = Path.GetExtension(xVal).ToUpper();
            if (xExt == ".XS") {
              xFiles.Add(Path.GetFullPath(xVal));
            } else if (xExt == ".DLL") {
              // TODO - Handle embedded resources .xs files
            }
            } else {
            throw new Exception("Not a valid file or directory: " + xVal);
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
