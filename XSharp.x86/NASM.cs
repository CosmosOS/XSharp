using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace XSharp.x86 {
    public class NASM : Assembler {
        protected readonly TextWriter mOut;

        public NASM(TextWriter aOut) {
            mOut = aOut;
        }

        public override void Emit(OpCode aOp, params object[] aParams) {
            throw new NotImplementedException();
        }
    }
}
