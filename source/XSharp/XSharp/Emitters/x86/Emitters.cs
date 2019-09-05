namespace XSharp.x86.Emitters
{
    public abstract class Emitters
    {
        protected Compiler Compiler { get; }
        protected x86.Assemblers.Assembler Asm { get; }

        protected Emitters(Compiler aCompiler, x86.Assemblers.Assembler aAsm)
        {
            Compiler = aCompiler;
            Asm = aAsm;
        }
    }
}
