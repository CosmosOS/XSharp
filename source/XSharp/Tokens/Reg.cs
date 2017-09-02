using System;
using System.Collections.Generic;
using System.Text;
using Spruce.Attribs;
using Spruce.Tokens;

namespace XSharp.Tokens {
    [GroupToken(typeof(Reg08), typeof(Reg16), typeof(Reg32))]
    public class Reg : AlphaNumList {
        protected Reg(string aText) : this(new[] { aText }) { }
        protected Reg(string[] aList) : base(aList) { }

        protected override object Transform(string aText) {
            return new x86.Register(aText);
        }
    }

    public class Reg08 : Reg {
        public Reg08() : base(x86.Register.Names.Reg08) { }
    }
    public class RegAL : Reg {
        public RegAL() : base("AL") { }
    }
    public class RegAH : Reg {
        public RegAH() : base("AH") { }
    }
    public class RegCL : Reg {
        public RegCL() : base("CL") { }
    }
    public class RegDX : Reg {
        public RegDX() : base("DX") { }
    }

    public class Reg16 : Reg {
        public Reg16() : base(x86.Register.Names.Reg16) { }
    }
    public class RegAX : Reg {
        public RegAX() : base("AX") { }
    }

    public class Reg32 : Reg {
        public Reg32() : base(x86.Register.Names.Reg32) { }
    }
    public class RegEAX : Reg {
        public RegEAX() : base("EAX") { }
    }
    public class RegECX : Reg {
        public RegECX() : base("ECX") { }
    }
    public class RegEDX : Reg {
        public RegEDX() : base("EDX") { }
    }

    // Section 6 - http://www.c-jump.com/CIS77/ASM/Addressing/lecture.html
    //public class BaseReg - use group
    //public class IndexReg - use group
}
