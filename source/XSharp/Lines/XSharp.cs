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
      if (Compiler.EmitSourceCode) {
        Compiler.WriteLine("; " + RawText.Trim());
      }

      TokenMap.Parse(RawText);
      // Where to emit? Part of parse? Internal seperation?
    }
  }
}
