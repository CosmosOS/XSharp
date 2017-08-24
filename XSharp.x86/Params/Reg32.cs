using System;
using System.Collections.Generic;
using System.Text;

namespace XSharp.x86.Params {
    public class Reg32 : RegXX {
        public static new readonly string[] Names = "EAX,EBX,ECX,EDX,ESI,EDI".Split(',');

        public Reg32() : base(Names) { }
        public Reg32(string[] aNames) : base(aNames) { }
    }
}
