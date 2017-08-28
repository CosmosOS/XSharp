using System;
using System.Collections.Generic;
using System.Text;
using Spruce.Attribs;
using Spruce.Tokens;

namespace XSharp.Tokens {
    public class Reg : AlphaNumList {
        protected Reg(string[] aList) : base(aList) { }

        public override object Check(string aText) {
            return new x86.Register(aText);
        }
    }

    [GroupToken(typeof(Reg08), typeof(Reg16), typeof(Reg32))]
    public class RegXX : Reg {
        protected RegXX(string[] aList) : base(aList) { }
    }

    public class Reg08 : RegXX {
        public Reg08() : base(x86.Register.Names.Reg08) { }
    }

    public class Reg16 : RegXX {
        public Reg16() : base(x86.Register.Names.Reg16) { }
    }

    public class Reg32 : RegXX {
        public Reg32() : base(x86.Register.Names.Reg32) { }
    }
}
