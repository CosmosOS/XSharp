using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XSharp.Tokens {
  public class Reg16 : Reg {
    public Reg16() : base(x86.Params.Reg16.Names) {
    }
  }
}
