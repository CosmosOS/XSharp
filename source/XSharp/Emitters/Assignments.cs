using Spruce.Attribs;
using Spruce.Tokens;
using XSharp.x86;
using XSharp.x86.Params;
using Reg = XSharp.Tokens.Reg;
using Reg08 = XSharp.Tokens.Reg08;
using Reg16 = XSharp.Tokens.Reg16;
using Reg32 = XSharp.Tokens.Reg32;

namespace XSharp.Emitters
{
    /// <summary>
    /// The class that provides assignments for different types.
    /// </summary>
    /// <seealso cref="XSharp.Emitters.Emitters" />
    public class Assignments : Emitters
    {
        public Assignments(Compiler aCompiler, x86.Assemblers.Assembler aAsm) : base(aCompiler, aAsm)
        {
        }

        // EAX = [EBX]
        [Emitter(typeof(Reg), typeof(OpEquals), typeof(OpOpenBracket), typeof(Reg), typeof(OpCloseBracket))]
        protected void MemoryAssignToReg(Register aRegister, string aOpEquals, string aOpOpenBracket, Register aSourceRegister, string aOpCloseBracket)
        {
            Asm.Emit(OpCode.Mov, aRegister, new Address(aSourceRegister));
        }

        [Emitter(typeof(Reg08), typeof(OpEquals), typeof(Reg08))] // AH = BH
        [Emitter(typeof(Reg16), typeof(OpEquals), typeof(Reg16))] // AX = BX
        [Emitter(typeof(Reg32), typeof(OpEquals), typeof(Reg32))] // EAX = EBX
        protected void RegAssigReg(Register aDestReg, string aEquals, Register aSrcReg)
        {
            Asm.Emit(OpCode.Mov, aDestReg, aSrcReg);
        }
    }
}
