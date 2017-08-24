using System;

namespace XSharp.x86 {
    public abstract class Assembler {
        public abstract void Emit(OpCode aOp, params object[] aParams);
    }
}
