using System;
using Spruce.Attribs;
using XSharp.Tokens;
using XSharp.x86;

namespace XSharp.x86.Emitters
{
    /// <summary>
    /// Class that performs increment and decrement assmebly operations
    /// </summary>
    /// <seealso cref="XSharp.Emitters.Emitters" />
    public class IncrementDecrement : Emitters
    {
        public IncrementDecrement(Compiler aCompiler, x86.Assemblers.Assembler aAsm) : base(aCompiler, aAsm)
        {
        }

        /// <summary>
        /// Increments the decrement register.
        /// </summary>
        [Emitter(typeof(Reg), typeof(OpIncDec))]
        protected void IncrementDecrementRegister(Register aRegister, string aOpIncrementDecrement)
        {
            switch (aOpIncrementDecrement)
            {
                case "++":
                    Asm.Emit(OpCode.Inc, aRegister);
                    break;

                case "--":
                    Asm.Emit(OpCode.Dec, aRegister);
                    break;

                default:
                    throw new Exception("Unsupported increment decrement operator");
            }
        }
    }
}
