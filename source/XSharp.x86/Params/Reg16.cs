using System;
using System.Collections.Generic;
using System.Text;

namespace XSharp.x86.Params {
    public class Reg16 : Reg {
        public static readonly string[] Names = "AX,BX,CX,DX".Split(',');

        public Reg16() : base(Names) { }
    }
}
