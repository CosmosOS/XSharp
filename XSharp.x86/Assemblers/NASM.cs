using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using XSharp.x86.Params;

namespace XSharp.x86.Assemblers {
    public class NASM : Assembler {
        protected readonly TextWriter mOut;
        protected readonly Map mRoot;

        public NASM(TextWriter aOut) {
            mOut = aOut;

            Param.ActionDelegate xAction = (Param[] aParams, object[] aValues) => {
                int x = 1;
            };
            mRoot = new Map();
            mRoot.Add(xAction, OpCode.Mov, typeof(RegXX), typeof(I32U));
        }

        public override void Emit(OpCode aOp, params object[] aParams) {
            throw new NotImplementedException();
        }
    }
}
