using System;
using System.Collections.Generic;
using System.Text;
using Spruce.Attribs;
using Spruce.Tokens;
using XSharp.Tokens;
using XSharp.x86;

namespace XSharp
{
    // Emitters does the actual translation from X# (via Spruce) to x86 (via Assemblers)
    public class Emitters
    {
        public readonly Compiler Compiler;
        public readonly x86.Assemblers.Assembler Asm;

        public Emitters(Compiler aCompiler, x86.Assemblers.Assembler aAsm)
        {
            Compiler = aCompiler;
            Asm = aAsm;
        }

        [Emitter(typeof(OpLiteral), typeof(All))] // //! Literal NASM Output
        protected void Literal(string aOp, string aText)
        {
            Compiler.WriteLine(aText);
        }

        [Emitter(typeof(OpComment), typeof(All))] // // Comment text
        protected void Comment(string aOp, string aText)
        {
            if (Compiler.EmitUserComments)
            {
                Compiler.WriteLine("; " + aText);
            }
        }

        [Emitter(typeof(Namespace), typeof(AlphaNum))] // namespace name
        protected void Namespace(string aNamespace, string aText)
        {
        }

        // Don't use RegXX. This method ensures proper data sizes.
        [Emitter(typeof(Reg08), typeof(OpEquals), typeof(Int08u))] // AH = 0
        [Emitter(typeof(Reg16), typeof(OpEquals), typeof(Int16u))] // AX = 0
        [Emitter(typeof(Reg32), typeof(OpEquals), typeof(Int32u))] // EAX = 0
        protected void RegAssignNum(string aReg, string aEquals, object aVal)
        {
            Asm.Emit(OpCode.Mov, aReg, aVal);
        }

        [Emitter(typeof(NOP))]
        [Emitter(typeof(Return))]
        [Emitter(typeof(PushAll))]
        [Emitter(typeof(PopAll))]
        protected void NOP(OpCode aOpCode)
        {
            Asm.Emit(aOpCode);
        }

        // +RegXX
        [Emitter(typeof(OpPlus), typeof(RegXX))]
        protected void RegPush(string aOp, string aReg) {
        }

        // -RegXX
        [Emitter(typeof(OpMinus), typeof(RegXX))]
        protected void RegPop(string aOp, string aReg) {
        }

        // if AL = #Vs2Ds_Noop return
        [Emitter(typeof(Reg08), typeof(OpEquals), typeof(Constant), typeof(Return))]
        protected void IfCondition(string register, string opEquals, string constantName, string opReturn)
        {
        }

        // Methods for functions. Leave these at the bottom of the file.

        // function fName123 {
        [Emitter(typeof(Function), typeof(AlphaNum), typeof(OpOpenBrace))]
        protected void FunctionDefinitionStart(string funcKeyword, string functionName, string opOpenBraces)
        {
        }

        // }
        [Emitter(typeof(OpCloseBrace))]
        protected void FunctionDefinitionEnd(string opCloseBrace)
        {
        }

        // Important! Last as fall through to prevent early claims over keywords.
        // fName ()
        [Emitter(typeof(AlphaNum), typeof(OpOpenParenthesis), typeof(OpCloseParenthesis))]
        protected void FunctionCall(string functionName, string opOpenParanthesis, string opCloseParanthesis)
        {
        }
    }
}
