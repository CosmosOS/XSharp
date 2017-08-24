using System;
using System.Collections.Generic;
using System.Text;

namespace XSharp.x86.Params {
    public abstract class Reg : List {
        protected Reg(string[] aNames) : base(aNames) { }
    }
}
