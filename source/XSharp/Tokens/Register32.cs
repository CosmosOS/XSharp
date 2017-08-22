using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XSharp.Tokens {
  public class Register32 : Register {
    public static readonly string[] Names = "EAX,EBX,ECX,EDX,ESI,EDI".Split(',');

    protected override object IsMatch(object aValue) {
      if (aValue is string && Names.Contains((string)aValue)) {
        return aValue;
      }
      return null;
    }
  }
}
