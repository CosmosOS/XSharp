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

    // EAX = 0
    [Emitter(typeof(Register), typeof(Assignment), typeof(Number64u))]
    protected string RegAssignNum(string aReg, string aEquals, UInt64 aVal) {
      return $"mov {aReg}, 0x{aVal:X}";
    }
  }
}
