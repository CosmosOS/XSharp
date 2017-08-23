using System;
using System.Collections.Generic;
using System.Text;
using XSharp.Tokens;

namespace XSharp {
  public class Emitters {
    protected readonly List<CodePoint> mCodePoints;

    public Emitters(List<CodePoint> aCodePoints) {
      mCodePoints = aCodePoints;
    }

    public class Op2Slash : Op {
      public Op2Slash() : base(@"//") {}
    }

    [Emitter(typeof(Op2Slash), typeof(All))]
    protected string Comment(string aOp, string aText) {
      return "; " + aText;
    }

    // EAX = 0
    [Emitter(typeof(RegXX), typeof(OpEquals), typeof(Number64u))]
    protected string RegAssignNum(string aReg, string aEquals, UInt64 aVal) {
      return $"mov {aReg}, 0x{aVal:X}";
    }
  }
}
