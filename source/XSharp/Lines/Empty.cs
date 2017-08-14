using System;
using System.Collections.Generic;
using System.Text;
using XSharp.Assembler;

namespace XSharp.Lines {
  public class Empty : Line {
    public Empty(Compiler aCompiler, string aLine) : base(aCompiler, aLine) {
    }

    public override void Emit() {
      Compiler.WriteLine();
    }
  }
}
