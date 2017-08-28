using System;
using System.Collections.Generic;
using System.Text;

namespace XSharp.x86.Params {
    public abstract class Reg : List {
        protected Reg(string[] aNames) : base(aNames) { }
    }

    public class Reg08 : Reg {
        public Reg08() : base(Register.Names.Reg08) { }
    }

    public class Reg16 : Reg {
        public Reg16() : base(Register.Names.Reg16) { }
    }

    public class Reg32 : Reg {
        public Reg32() : base(Register.Names.Reg32) { }
    }
}
