using Spruce.Attribs;
using XSharp.Tokens;
using XSharp.x86;

namespace XSharp.Emitters
{
    /// <summary>
    /// Operations without any parameters
    /// </summary>
    /// <seealso cref="XSharp.Emitters.Emitters" />
    public class ZeroParamOps : Emitters
    {
        public ZeroParamOps(Compiler aCompiler, x86.Assemblers.Assembler aAsm) : base(aCompiler, aAsm)
        {
        }

        [Emitter(typeof(NOP))]
        [Emitter(typeof(Return))]
        [Emitter(typeof(PushAll))]
        [Emitter(typeof(PopAll))]
        protected void ZeroParamOp(OpCode aOpCode)
        {
            Asm.Emit(aOpCode);
        }
    }
}
