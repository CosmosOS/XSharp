using System;
using System.IO;
using XSharp.Assembler;
using XSharp;

namespace XSharp.Compiler {
  class Program {
    static void Main(string[] aArgs) {
      try {
        if (aArgs.Length == 0) {
          throw new Exception("No arguments were specified.");
        }

        string xSrc = aArgs[0];
        var xGenerator = new AsmGenerator();

        string[] xFiles;
        if (Directory.Exists(xSrc)) {
          xFiles = Directory.GetFiles(xSrc, "*.xs");
        } else {
          xFiles = new string[] { xSrc };
        }
        foreach (var xFile in xFiles) {
          xGenerator.GenerateToFiles(xFile);
        }

        Console.WriteLine("Done.");
      } catch (Exception ex) {
        Console.WriteLine(ex.ToString());
        System.Threading.Thread.Sleep(3000);
        Environment.Exit(1);
      }
    }
  }
}
