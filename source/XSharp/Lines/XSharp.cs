using System;
using System.Collections.Generic;
using System.Text;

namespace XSharp.Lines {
  public class XSharp : Line {
    public XSharp(Compiler aCompiler, string aLine) : base(aCompiler, aLine) {
    }

    public override void Emit() {
      if (Compiler.EmitSourceCode) {
        Compiler.WriteLine("; " + RawText.Trim());
      }
    }
  }
}
