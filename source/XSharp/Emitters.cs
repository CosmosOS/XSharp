using System;
using System.Collections.Generic;
using System.Text;
using XSharp.Tokens;
using Spruce.Attribs;

namespace XSharp {
  public class Emitters {
    protected readonly Compiler mCompiler;
    protected readonly List<CodePoint> mCodePoints;

    public Emitters(Compiler aCompiler, List<CodePoint> aCodePoints) {
      mCompiler = aCompiler;
      mCodePoints = aCodePoints;
    }

    public class OpComment : Op {
      public OpComment() : base(@"//") { }
    }
    public class OpLiteral : Op {
      public OpLiteral() : base(@"//!") { }
    }

    [Emitter(typeof(OpLiteral), typeof(All))]
    protected string Literal(string aOp, string aText) {
      return aText;
    }

    [Emitter(typeof(OpComment), typeof(All))]
    protected string Comment(string aOp, string aText) {
      if (mCompiler.EmitUserComments) {
        mCompiler.WriteLine("; " + aText);
      }
      return "; " + aText;
    }

    // EAX = 0
    [Emitter(typeof(RegXX), typeof(OpEquals), typeof(Number64u))]
    protected string RegAssignNum(string aReg, string aEquals, UInt64 aVal) {
      return $"mov {aReg}, 0x{aVal:X}";
    }
  }
}
