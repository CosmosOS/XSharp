using System;
using System.Collections.Generic;
using System.Text;

namespace XSharp.x86.Params {
    public class Reg08 : Reg {
        public static readonly string[] Names = "AH,AL,BH,BL,CH,CL,DH,DL".Split(',');

        public Reg08() : base(Names) { }
    }
}
