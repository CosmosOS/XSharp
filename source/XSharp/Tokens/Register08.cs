using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XSharp.Tokens {
  public class Register08 : RegisterBase {
    public static readonly string[] Names = "AH,AL,BH,BL,CH,CL,DH,DL".Split(',');

    public Register08() {
      Size = 8;
    }

    protected override bool IsMatch(Values.Value aValue) {
      return Names.Contains(aValue.RawText.ToUpper());
    }
  }
}
