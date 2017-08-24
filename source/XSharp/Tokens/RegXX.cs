using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XSharp.Tokens {
  public class RegXX : Reg {
    // MUST be overloads and not default param. See note in Reg.
    public RegXX() : base(x86.Params.RegXX.Names) { }
    public RegXX(string[] aList) : base(aList) { }
  }
}
