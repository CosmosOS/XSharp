using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XSharp.Tokens {
  public class Reg08 : Reg {
    public static readonly string[] Names = "AH,AL,BH,BL,CH,CL,DH,DL".Split(',');

    protected override string[] GetList() {
      return Names;
    }
  }
}
