using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace XSharp.Lines {
  public class XSharp : Line {
    protected static Tokens.Root mTokenMap = new Tokens.Root();

    public XSharp(Compiler aCompiler, string aLine) : base(aCompiler, aLine) {
    }

    public override void Emit() {
      if (Compiler.EmitSourceCode) {
        Compiler.WriteLine("; " + RawText.Trim());
      }

      var xCodePoints = mTokenMap.Parse(RawText);
      var xLastToken = xCodePoints.Last().Token;
      xLastToken.Emitter(Compiler, xCodePoints);
    }
  }
}
