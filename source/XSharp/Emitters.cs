using System;
using System.Collections.Generic;
using System.Text;
using Spruce.Attribs;
using Spruce.Tokens;
using XSharp.Tokens;
using XSharp.x86;
using XSharp.x86.Assemblers;

namespace XSharp {
  public class Emitters {
    public readonly Compiler Compiler;
    public readonly x86.Assemblers.Assembler Asm;

    public Emitters(Compiler aCompiler) {
      Compiler = aCompiler;
      Asm = Compiler.Asm;
    }

    // //! NASM Mnemonic
    [Emitter(typeof(OpLiteral), typeof(All))]
    protected void Literal(string aOp, string aText) {
      Compiler.WriteLine(aText);
    }

    // // Texxt
    [Emitter(typeof(OpComment), typeof(All))]
    protected void Comment(string aOp, string aText) {
      if (Compiler.EmitUserComments) {
        Compiler.WriteLine("; " + aText);
      }
      Compiler.WriteLine("; " + aText);
    }

    // EAX = 0
    [Emitter(typeof(RegXX), typeof(OpEquals), typeof(Number32u))]
    protected void RegAssignNum(string aReg, string aEquals, UInt32 aVal) {
      Compiler.WriteLine($"mov {aReg}, 0x{aVal:X}");

      Asm.Emit(OpCode.Mov, aReg, aVal);
    }
  }
}
