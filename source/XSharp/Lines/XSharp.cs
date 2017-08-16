using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Linq;

namespace XSharp.Lines {
  public class XSharp : Line {
    protected Tokens.Root TokenMap = new Tokens.Root();

    public XSharp(Compiler aCompiler, string aLine) : base(aCompiler, aLine) {
    }

    public override void Emit() {
      string xText = RawText.Trim();
      if (Compiler.EmitSourceCode) {
        Compiler.WriteLine("; " + xText);
      }

      TokenMap.Parse(xText);
      // Where to emit? Part of parse? Internal seperation?
    }
  }
}
