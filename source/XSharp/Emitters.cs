using System;
using System.Collections.Generic;
using System.Text;

namespace XSharp {
  public class Emitters {
    protected readonly Compiler mCompiler;
    protected readonly List<CodePoint> mCodePoints;
    public Emitters(Compiler aCompiler, List<CodePoint> aCodePoints) {
      mCompiler = aCompiler;
      mCodePoints = aCodePoints;
    }
  }
}
