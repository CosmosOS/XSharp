namespace XSharp.x86.Assemblers
{
    public abstract class Assembler {
        public abstract void Emit(OpCode aOp, params object[] aParams);
    }
}
