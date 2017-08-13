using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Loader;

namespace XSharp.DotNetCLI {
  class Program {
    static void Main(string[] aArgs) {
      try {
        var xGen = new AsmGenerator();

        // Parse arguments
        var xCLI = new Build.CliProcessor();
        xCLI.Parse(aArgs);

        // Options
        var xUserComments = xCLI.GetSwitch("UserComments", "UC");
        if (xUserComments != null) {
          xGen.EmitUserComments = xUserComments.Check("ON", new string[] { "ON", "OFF" }) == "ON";
        }
        //
        var xSourceCode = xCLI.GetSwitch("SourceCode", "SC");
        if (xSourceCode != null) {
          xGen.EmitSourceCode = xSourceCode.Check("ON", new string[] { "ON", "OFF" }) == "ON";
        }
        //
        var xOutput = xCLI.GetSwitch("Out", "O");
        if (xOutput != null) {
          // Instead of using individual .asm output files, use one output file as specified in xOutput.Value
          // If file already exists, throw exception. Check for exception in XSharp itself and not here.
        }
        //
        var xAppend = xCLI.GetSwitch("Append", "A");
        if (xAppend != null) {
          if (xOutput != null) {
            throw new Exception("Output and Append cannot be specified at the same time.");
          }
          // Instead of using individual .asm output files, append output to existing file. If file does not exist, create it.
        }

        // Plugins
        var xPlugins = xCLI.GetSwitches("PlugIn");
        foreach (var xPlugin in xPlugins) {
          // TODO Load plugins
        }

        // List of source files
        var xFiles = new List<string>();
        var xAssemblies = new List<Assembly>();
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
              xAssemblies.Add(AssemblyLoadContext.Default.LoadFromAssemblyPath(xVal));
            } else {
              throw new Exception("Not a valid file type: " + xVal);
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

        // Generate output from embedded resources
        foreach (var xAssembly in xAssemblies)
        {
          var xDestination = Path.ChangeExtension(xAssembly.Location, "asm");
          var xWriter = new StreamWriter(File.Create(xDestination));

          foreach (var xResource in xAssembly.GetManifestResourceNames()
                                             .Where(r => r.EndsWith(".xs", StringComparison.OrdinalIgnoreCase)))
          {
            var xStream = xAssembly.GetManifestResourceStream(xResource);
            xGen.GenerateFromEmbeddedResource(xResource, xStream, xWriter);
          }
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
