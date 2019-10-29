using Spruce.Attribs;
using XSharp.Tokens;
using XSharp.x86;

namespace XSharp.x86.Emitters
{
    /// <summary>
    /// Operations without any parameters
    /// </summary>
    /// <seealso cref="XSharp.x86.Emitters.Emitters" />
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
            if (aOpCode == OpCode.Ret && Compiler.Blocks.Current()?.Type == Compiler.BlockType.If)
            {
                Asm.Emit(OpCode.Jmp, Compiler.CurrentFunctionExitLabel);
                return;
            }

            Asm.Emit(aOpCode);
        }
    }
}
