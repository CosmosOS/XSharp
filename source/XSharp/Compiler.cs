using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;
using System.Reflection;
using System.Text;

namespace XSharp {
  public class Compiler {
    protected readonly TextWriter mOut;
    protected int mLineNo;

    public Compiler(TextWriter aOut) {
      mOut = aOut;
    }

    public void Generate(TextReader aIn) {
      mLineNo = 1;
      string xText = aIn.ReadLine();
      while (xText != null) {
        var xLine = Lines.Line.New(xText);

        xText = aIn.ReadLine();
        mLineNo++;
      }
    }
  }
}
