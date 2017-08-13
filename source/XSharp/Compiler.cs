using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;
using System.Reflection;
using System.Text;

namespace XSharp {
  public class Compiler {
    protected readonly TextWriter Out;
    public int LineNo { get; private set; }

    public Compiler(TextWriter aOut) {
      Out = aOut;
    }

    public void Generate(TextReader aIn) {
      try {
        LineNo = 1;
        string xText = aIn.ReadLine();
        while (xText != null) {
          var xLine = Lines.Line.New(this, xText);

          xText = aIn.ReadLine();
          LineNo++;
        }
      } catch (Exception e) {
        throw new Exception("Generation error on line " + LineNo, e);
      }
    }
  }
}
