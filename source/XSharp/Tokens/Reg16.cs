using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XSharp.Tokens {
  public class Reg16 : Reg {
    public static readonly string[] Names = "AX,BX,CX,DX".Split(',');

    protected override string[] GetList() {
      return Names;
    }
  }
}
