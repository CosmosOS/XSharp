using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using XSharp.x86.Params;

namespace XSharp.x86 {
    public class NASM : Assembler {
        protected readonly TextWriter mOut;
        protected readonly Root mRoot;

        public NASM(TextWriter aOut) {
            mOut = aOut;

            Root.Action xAction = (List<Param> aParams) => {
                int x = 1;
            };
            mRoot = new Root();
            mRoot.Add(xAction, OpCode.Mov, typeof(RegXX), typeof(I32U));
        }

        public override void Emit(OpCode aOp, params object[] aParams) {
            throw new NotImplementedException();
        }
    }
}
