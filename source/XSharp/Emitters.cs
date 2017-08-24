using System;
using System.Collections.Generic;
using System.Text;
using Spruce;
using Spruce.Attribs;
using Spruce.Tokens;
using XSharp.Tokens;
using XSharp.x86;

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
    protected void Literal(string aOp, string aText) {
      mCompiler.WriteLine(aText);
    }

    [Emitter(typeof(OpComment), typeof(All))]
    protected void Comment(string aOp, string aText) {
      if (mCompiler.EmitUserComments) {
        mCompiler.WriteLine("; " + aText);
      }
      mCompiler.WriteLine("; " + aText);
    }

    // EAX = 0
    [Emitter(typeof(RegXX), typeof(OpEquals), typeof(Number64u))]
    protected void RegAssignNum(string aReg, string aEquals, UInt64 aVal) {
      var xAsm = new NASM();
      xAsm.Emit(OpCode.Mov, aReg, aVal);

      mCompiler.WriteLine($"mov {aReg}, 0x{aVal:X}");
    }
  }
}
