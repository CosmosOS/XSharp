using System;
using System.Collections.Generic;
using System.Text;

namespace XSharp.x86.Params {
    public class Reg08 : RegXX {
        public static new readonly string[] Names = "AH,AL,BH,BL,CH,CL,DH,DL".Split(',');

        public Reg08() : base(Names) { }
        public Reg08(string[] aNames) : base(aNames) { }
    }
}
