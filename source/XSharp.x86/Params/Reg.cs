using System;
using System.Collections.Generic;
using System.Text;

namespace XSharp.x86.Params {
    public abstract class Reg : List {
        protected Reg(string[] aNames) : base(aNames) { }
    }

    public class Reg08 : Reg {
        public static readonly string[] Names = "AH,AL,BH,BL,CH,CL,DH,DL".Split(',');

        public Reg08() : base(Names) { }
    }

    public class Reg16 : Reg {
        public static readonly string[] Names = "AX,BX,CX,DX".Split(',');

        public Reg16() : base(Names) { }
    }

    public class Reg32 : Reg {
        public static readonly string[] Names = "EAX,EBX,ECX,EDX,ESI,EDI".Split(',');

        public Reg32() : base(Names) { }
    }
}
