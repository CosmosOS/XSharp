using Spruce.Attribs;
using Spruce.Tokens;
using XSharp.Tokens;
using XSharp.x86;

namespace XSharp.Emitters
{
    /// <summary>
    /// IN and OUT functions from assembly <br/>
    /// This instruction is only useful for accessing I/O ports located in the processor's I/O address space. <br/>
    /// See Chapter 13, Input/Output, in the IA-32 Intel Architecture Software Developer's Manual, Volume 1,
    /// for more information on accessing I/O ports in the I/O address space.
    /// </summary>
    /// <seealso cref="XSharp.Emitters.Emitters" />
    public class Ports : Emitters
    {
        public Ports(Compiler aCompiler, x86.Assemblers.Assembler aAsm) : base(aCompiler, aAsm)
        {
        }

        // ===============================================================
        // PORT

        // Port[x] = EAX/AX/AL
        [Emitter(typeof(PortKeyword), typeof(OpOpenBracket), typeof(Int08u), typeof(OpCloseBracket), typeof(OpEquals), typeof(Reg))]
        protected void PortOut(string aPortKeyword, string aOpOpenBracket, byte aPortNo, string aOpCloseBracket, string aOpEquals, Register aSrcReg)
        {
            aSrcReg.CheckIsAccumulator();

            Asm.Emit(OpCode.Out, aPortNo, aSrcReg);
        }

        // Port[DX] = EAX/AX/AL
        [Emitter(typeof(PortKeyword), typeof(OpOpenBracket), typeof(Reg16), typeof(OpCloseBracket), typeof(OpEquals), typeof(Reg))]
        protected void PortOut(string aPortKeyword, string aOpOpenBracket, Register aPortReg, string aOpCloseBracket, string aOpEquals, Register aSrcReg)
        {
            aPortReg.CheckIsDX();
            aSrcReg.CheckIsAccumulator();

            Asm.Emit(OpCode.Out, aPortReg, aSrcReg);
        }

        // EAX/AX/AL = Port[x]
        [Emitter(typeof(Reg), typeof(OpEquals), typeof(PortKeyword), typeof(OpOpenBracket), typeof(Int08u), typeof(OpCloseBracket))]
        protected void PortIn(Register aDestReg, string aOpEquals, string aPortKeyword, string aOpOpenBracket, byte aPortNo, string aOpCloseBracket)
        {
            aDestReg.CheckIsAccumulator();

            Asm.Emit(OpCode.In, aDestReg, aPortNo);
        }

        // EAX/AX/AL = Port[DX]
        [Emitter(typeof(Reg), typeof(OpEquals), typeof(PortKeyword), typeof(OpOpenBracket), typeof(Reg16), typeof(OpCloseBracket))]
        protected void PortIn(Register aDestReg, string aOpEquals, string aPortKeyword, string aOpOpenBracket, Register aPortReg, string aOpCloseBracket)
        {
            aDestReg.CheckIsAccumulator();
            aPortReg.CheckIsDX();

            Asm.Emit(OpCode.In, aDestReg, aPortReg);
        }
    }
}
