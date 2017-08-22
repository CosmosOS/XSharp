using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XSharp.Tokens {
  public class Register16 : Register {
    public static readonly string[] Names = "AX,BX,CX,DX".Split(',');

    protected override object IsMatch(object aValue) {
      if (aValue is string && Names.Contains((string)aValue)) {
        return aValue;
      }
      return null;
    }
  }
}
