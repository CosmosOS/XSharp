using System;
using System.Collections.Generic;
using System.Text;

namespace XSharp.Lines {
  public class Literal : Line {
    public Literal(Compiler aCompiler, string aLine) : base(aCompiler, aLine) {
    }

    public override void Emit() {
      Compiler.WriteLine(RawText);
    }
  }
}
