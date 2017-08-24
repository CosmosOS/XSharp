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
      // Parse arguments
      var xCLI = new Build.CliProcessor();
      xCLI.Parse(aArgs);

      try {
        var xGen = new AsmGenerator();

        bool xAppend = false;
        string xOutputPath = null;

        // Options
        var xUserComments = xCLI["UserComments", "UC"];
        if (xUserComments != null) {
          xGen.EmitUserComments = xUserComments.Check("ON", new string[] { "ON", "OFF" }) == "ON";
        }
        //
        var xSourceCode = xCLI["SourceCode", "SC"];
        if (xSourceCode != null) {
          xGen.EmitSourceCode = xSourceCode.Check("ON", new string[] { "ON", "OFF" }) == "ON";
        }
        //
        var xOutput = xCLI["Out", "O"];
        if (xOutput != null) {
          xOutputPath = xOutput.Value;
        }
        //
        xAppend = xCLI["Append", "A"] != null;
        if (xAppend && xOutput == null) {
          throw new Exception("Use of -Append requires use of -Out.");
        }

        // Plugins
        var xPlugins = xCLI.GetSwitches("PlugIn");
        foreach (var xPlugin in xPlugins) {
          // TODO Load plugins
        }

        // List of source files
        var xFiles = new List<string>();
        var xAssemblies = new List<Assembly>();
        foreach (var xArg in xCLI.Args) {
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

        if (xCLI["Gen2"] != null) {
          foreach (var xFile in xFiles) {
            using (var xIn = File.OpenText(xFile)) {
              using (var xOut = File.CreateText(Path.ChangeExtension(xFile, ".asm"))) {
                var xCompiler = new Compiler(xOut);
                xCompiler.Emit(xIn);
              }
            }
          }
        } else {
          // Generate output
          foreach (var xFile in xFiles) {
            var xReader = File.OpenText(xFile);
            if (xAppend) {
              xGen.Generate(xReader, File.AppendText(xOutputPath));
            } else if (xFiles.Count == 1 && xOutputPath != null) {
              xGen.Generate(File.OpenText(xFile), File.CreateText(xOutputPath));
            } else {
              Console.WriteLine(xFile);
              xGen.GenerateToFiles(xFile);
            }
          }

          // Generate output from embedded resources
          foreach (var xAssembly in xAssemblies) {
            TextWriter xWriter = null;
            if (!xAppend) {
              var xDestination = Path.ChangeExtension(xAssembly.Location, "asm");
              xWriter = new StreamWriter(File.Create(xDestination));
            }

            foreach (var xResource in xAssembly.GetManifestResourceNames()
              .Where(r => r.EndsWith(".xs", StringComparison.OrdinalIgnoreCase))) {
              var xStream = xAssembly.GetManifestResourceStream(xResource);
              xGen.GenerateToFile(xResource, new StreamReader(xStream), xWriter);
            }
          }
        }

        // Finalize
        Console.WriteLine("Done.");
        } catch (Exception ex) {
          Console.WriteLine(ex.ToString());
          if (xCLI["WaitOnError"] != null) {
            Console.WriteLine();
            Console.WriteLine("Waiting on error. Press Enter to exit.");
            Console.ReadLine();
          }
          Environment.Exit(-1);
        }
      } catch (Exception ex) {
        Console.WriteLine("Argument parse error:\r\n" + ex);
        Environment.Exit(-2);
      }
    }
  }
}
