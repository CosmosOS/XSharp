using System;
using System.Collections.Generic;
using System.Text;
using Spruce.Tokens;

namespace XSharp.Tokens {
  public abstract class Reg : IdList {
    // Must use overloads. Optional param is shorter but does not provide a
    // parameterless ctor for Activator.
    public Reg() : base(x86.Params.RegXX.Names) { }
    public Reg(string[] aList) : base(aList) { }
  }
}
