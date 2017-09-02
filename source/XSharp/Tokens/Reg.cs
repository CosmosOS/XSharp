using System;
using System.Collections.Generic;
using System.Text;
using Spruce.Attribs;
using Spruce.Tokens;

namespace XSharp.Tokens {
    [GroupToken(typeof(Reg08), typeof(Reg16), typeof(Reg32))]
    public class Reg : AlphaNumList {
        protected Reg(string[] aList) : base(aList) { }

        protected override object Transform(string aText) {
            return new x86.Register(aText);
        }
    }

    public class Reg08 : Reg {
        public Reg08() : base(x86.Register.Names.Reg08) { }
    }

    public class Reg16 : Reg {
        public Reg16() : base(x86.Register.Names.Reg16) { }
    }

    public class Reg32 : Reg {
        public Reg32() : base(x86.Register.Names.Reg32) { }
    }

    // Section 6 - http://www.c-jump.com/CIS77/ASM/Addressing/lecture.html
    //public class BaseReg - use group
    //public class IndexReg - use group
}
