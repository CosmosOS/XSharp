using Spruce.Attribs;
using Spruce.Tokens;
using XSharp.Tokens;
using XSharp.x86;
using XSharp.x86.Params;
using Reg = XSharp.Tokens.Reg;

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
        [Emitter(typeof(Reg), typeof(OpEquals), typeof(RegAddr))]
        protected void MemoryAssignToReg(Register aRegister, string aOpEquals, Address registerAddress)
        {
            Asm.Emit(OpCode.Mov, aRegister, registerAddress);
        }
    }
}
