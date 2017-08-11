using System;
using System.IO;
using XSharp.Assembler;
using XSharp;

namespace XSharp.Compiler {
  class Program {
    static void Main(string[] aArgs) {
      try {
        string xSrc = aArgs[0];
        var xGenerator = new AsmGenerator();

        // TODO - reenable this as a switch - ie all in dir.
        //string[] xFiles;
        //if (Directory.Exists(xSrc))
        //{
        //  xFiles = Directory.GetFiles(xSrc, "*.xs");
        //}
        //else
        //{
        //  xFiles = new string[] { xSrc };
        //}
        //foreach (var xFile in xFiles)
        //{
        //  xGenerator.GenerateToFiles(xFile);
        //}

        var xAsm = new Assembler.Assembler();
        var xStreamReader = new StringReader(@"namespace Test
            while byte ESI[0] != 0 {
              ! nop
            }
            ");
        var xResult = xGenerator.Generate(xStreamReader);
        Console.WriteLine("done");
      } catch (Exception ex) {
        Console.WriteLine(ex.ToString());
        Environment.Exit(1);
      }
    }
  }
}
