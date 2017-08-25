using System;
using System.Collections.Generic;
using System.Text;
using Spruce.Tokens;

namespace XSharp.Tokens {
  public class Reg : Identifier {
    protected Reg(string[] aList) : base(aList) { }
  }

  public class Reg08 : Reg {
    public Reg08() : base(x86.Params.Reg08.Names) { }
  }

  public class Reg16 : Reg {
    public Reg16() : base(x86.Params.Reg16.Names) { }
  }

  public class Reg32 : Reg {
    public Reg32() : base(x86.Params.Reg32.Names) { }
  }
}
