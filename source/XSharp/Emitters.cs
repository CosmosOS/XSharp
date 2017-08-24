using System;
using System.Collections.Generic;
using System.Text;
using Spruce.Attribs;
using Spruce.Tokens;
using XSharp.Tokens;
using XSharp.x86;
using XSharp.x86.Assemblers;

namespace XSharp {
  // Emitters does the actual translation from X# (via Spruce) to x86 (via Assemblers)
  public class Emitters {
    public readonly Compiler Compiler;
    public readonly x86.Assemblers.Assembler Asm;

    public Emitters(Compiler aCompiler, x86.Assemblers.Assembler aAsm) {
      Compiler = aCompiler;
      Asm = aAsm;
    }

    // //! NASM Mnemonic
    [Emitter(typeof(OpLiteral), typeof(All))]
    protected void Literal(string aOp, string aText) {
      Compiler.WriteLine(aText);
    }

    // // Text
    [Emitter(typeof(OpComment), typeof(All))]
    protected void Comment(string aOp, string aText) {
      if (Compiler.EmitUserComments) {
        Compiler.WriteLine("; " + aText);
      }
    }

    // EAX = 0
    [Emitter(typeof(Reg32), typeof(OpEquals), typeof(Num32u))]
    protected void RegAssignNum(string aReg, string aEquals, UInt32 aVal) {
      Asm.Emit(OpCode.Mov, aReg, aVal);
    }
  }
}
