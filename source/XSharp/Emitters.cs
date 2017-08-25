using System;
using System.Collections.Generic;
using System.Text;
using Spruce.Attribs;
using Spruce.Tokens;
using XSharp.Tokens;
using XSharp.x86;

namespace XSharp {
  // Emitters does the actual translation from X# (via Spruce) to x86 (via Assemblers)
  public class Emitters {
    public readonly Compiler Compiler;
    public readonly x86.Assemblers.Assembler Asm;

    public Emitters(Compiler aCompiler, x86.Assemblers.Assembler aAsm) {
      Compiler = aCompiler;
      Asm = aAsm;
    }

    [Emitter(typeof(OpLiteral), typeof(All))] // //! NASM Mnemonic
    protected void Literal(string aOp, string aText) {
      Compiler.WriteLine(aText);
    }

    [Emitter(typeof(OpComment), typeof(All))] // // Text
    protected void Comment(string aOp, string aText) {
      if (Compiler.EmitUserComments) {
        Compiler.WriteLine("; " + aText);
      }
    }

    [Emitter(typeof(Namespace), typeof(Identifier))] // namespace name
    protected void Namespace(string aNamespace, string aText) {
    }

    [Emitter(typeof(Reg08), typeof(OpEquals), typeof(Num08u))] // AH = 0
    [Emitter(typeof(Reg16), typeof(OpEquals), typeof(Num16u))] // AX = 0
    [Emitter(typeof(Reg32), typeof(OpEquals), typeof(Num32u))] // EAX = 0
    protected void RegAssignNum(string aReg, string aEquals, object aVal) {
      Asm.Emit(OpCode.Mov, aReg, aVal);
    }
  }
}
