using System;
using System.Collections.Generic;
using System.Text;

namespace XSharp.x86.Params {
    public class Reg16 : RegXX {
        public static new readonly string[] Names = "AX,BX,CX,DX".Split(',');

        public Reg16() : base(Names) { }
        public Reg16(string[] aNames) : base(aNames) { }
    }
}
