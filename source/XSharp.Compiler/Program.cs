using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace XSharp.CommandLine {
  internal class Program {
    private static void Main(string[] aArgs) {
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
          if (xUserComments != null) xGen.EmitUserComments = xUserComments.Check("ON", new string[] { "ON", "OFF" }) == "ON";
          //
          var xSourceCode = xCLI["SourceCode", "SC"];
          if (xSourceCode != null) xGen.EmitSourceCode = xSourceCode.Check("ON", new string[] { "ON", "OFF" }) == "ON";
          //
          var xOutput = xCLI["Out", "O"];
          if (xOutput != null) xOutputPath = xOutput.Value;
          //
          xAppend = xCLI["Append", "A"] != null;
          if (xAppend && xOutput == null) throw new Exception("Use of -Append requires use of -Out.");

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
                xAssemblies.Add(Assembly.LoadFrom(xVal));
              } else {
                throw new Exception("Not a valid file type: " + xVal);
              }
              
            } else {
              throw new Exception("Not a valid file or directory: " + xVal);
            }
          }

          if (xCLI["Gen2"] != null) {
            foreach (var xFile in xFiles) {
              using(var xIn = File.OpenText(xFile)) {
                if (!xAppend) xOutputPath = Path.ChangeExtension(xFile, ".asm");

                using(var xOut = File.CreateText(xOutputPath)) {
                  Console.WriteLine("Processing file: " + xFile);
                  Compiler xCompiler;

                  try {
                    xCompiler = new Compiler(xOut);
                  } catch (Exception ex) {
                    Console.WriteLine(ex);
                    throw;
                  }

                  var xOrigBgColor = Console.BackgroundColor;
                  try {
                    xCompiler.Emit(xIn);

                    Console.BackgroundColor = ConsoleColor.Green;
                    Console.WriteLine("File processed");
                    Console.BackgroundColor = xOrigBgColor;
                  } catch (Exception ex) {
                    Console.BackgroundColor = ConsoleColor.Red;
                    Console.WriteLine(ex);
                    Console.BackgroundColor = xOrigBgColor;
                    throw;
                  }
                }
              }
            }

          } else {
            try {
              // Generate output
              foreach (var xFile in xFiles) {
                var xReader = File.OpenText(xFile);
                if (xAppend) {
                  xGen.GenerateToFile(xFile, xReader, File.AppendText(xOutputPath));
                } else if (xFiles.Count == 1 && xOutputPath != null) {
                  xGen.GenerateToFile(xFile, File.OpenText(xFile), File.CreateText(xOutputPath));
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

                var xResources = xAssembly.GetManifestResourceNames().Where(r => r.EndsWith(".xs", StringComparison.OrdinalIgnoreCase));
                foreach (var xResource in xResources) {
                  var xStream = xAssembly.GetManifestResourceStream(xResource);
                  xGen.GenerateToFile(xResource, new StreamReader(xStream), xWriter);
                }
              }
            } catch (Exception ex) {
              Console.WriteLine(ex);
            }
          }

          // Finalize
          Console.WriteLine("Done.");
        } catch (Exception ex) {
          if (xCLI["WaitOnError"] != null) {
            Console.WriteLine();
            Console.WriteLine("Waiting on error. Press Enter to exit.");
            Console.WriteLine("Exception: " + ex.ToString());
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