using System;
using System.Collections.Generic;
using System.Text;
using Spruce.Tokens;
using x86P = XSharp.x86.Params;

namespace XSharp.Tokens {
    public class Reg : MatchList {
        protected Reg(string[] aList) : base(aList) { }
        protected Reg(string[][] aList) : base(aList) { }
    }

    public class RegXX : Reg {
        public RegXX() : base(new string[][] { x86P.Reg08.Names, x86P.Reg16.Names, x86P.Reg32.Names }) { }
        protected RegXX(string[] aList) : base(aList) { }
    }

    public class Reg08 : RegXX {
        public Reg08() : base(x86P.Reg08.Names) { }
    }

    public class Reg16 : RegXX {
        public Reg16() : base(x86P.Reg16.Names) { }
    }

    public class Reg32 : RegXX {
        public Reg32() : base(x86P.Reg32.Names) { }
    }
}
