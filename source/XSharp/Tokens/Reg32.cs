using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XSharp.Tokens {
  public class Reg32 : Reg {
    protected override string[] GetList() {
      return x86.Params.Reg32.Names;
    }
  }
}
