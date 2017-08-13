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

        bool xAppend = false;
        string xOutputPath = null;

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
          xOutputPath = xOutput.Value;
        }
        //
        xAppend = xCLI.GetSwitch("Append", "A") != null;
        if (xAppend && xOutput == null)
        {
          throw new Exception("When append is specified, output must be specified too!");
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

        StreamWriter xWriter = null;
        if (xAppend)
        {
          xWriter = new StreamWriter(File.Create(xOutputPath));
        }

        // Generate output
        foreach (var xFile in xFiles) {
          if (xAppend)
          {
            var xReader = new StreamReader(File.OpenRead(xFile));
            xGen.GenerateToFile(Path.GetFileName(xFile), xReader, xWriter);
          }
          else if (xFiles.Count == 1 && xOutputPath != null)
          {
            xGen.Generate(new StreamReader(File.OpenRead(xFile)), xWriter);
          }
          else
          {
            Console.WriteLine(xFile);
            xGen.GenerateToFiles(xFile);
          }
        }

        // Generate output from embedded resources
        foreach (var xAssembly in xAssemblies)
        {
          if (!xAppend)
          {
            var xDestination = Path.ChangeExtension(xAssembly.Location, "asm");
            xWriter = new StreamWriter(File.Create(xDestination));
          }

          foreach (var xResource in xAssembly.GetManifestResourceNames()
                                             .Where(r => r.EndsWith(".xs", StringComparison.OrdinalIgnoreCase)))
          {
            var xStream = xAssembly.GetManifestResourceStream(xResource);
            xGen.GenerateToFile(xResource, new StreamReader(xStream), xWriter);
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
