using System.Collections.Generic;
using System;
using System.Text;

namespace XSharp.Lines {
  public class Comment : Line {
    public Comment(Compiler aCompiler, string aLine) : base(aCompiler, aLine) {
    }

    public override void Emit() {
      if (Compiler.EmitUserComments) {
        Compiler.WriteLine("; " + RawText);
      }
    }
  }
}
