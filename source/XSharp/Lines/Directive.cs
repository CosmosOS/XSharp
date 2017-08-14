using System;
using System.Collections.Generic;
using System.Text;

namespace XSharp.Lines {
  public class Directive : Line {
    public Directive(Compiler aCompiler, string aLine) : base(aCompiler, aLine) {
    }

    public override void Emit() {
    }
  }
}
