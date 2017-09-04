namespace XSharp.Emitters
{
    public abstract class Emitters
    {
        public readonly Compiler Compiler;
        public readonly x86.Assemblers.Assembler Asm;

        protected Emitters(Compiler aCompiler, x86.Assemblers.Assembler aAsm)
        {
            Compiler = aCompiler;
            Asm = aAsm;
        }
    }
}
