using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XSharp.Tokens {
  public class Register32 : RegisterBase {
    public static readonly string[] Names = "EAX,EBX,ECX,EDX,ESI,EDI".Split(',');

    public Register32() {
      Size = 32;
    }

    protected override bool IsMatch(string aText) {
      return Names.Contains(aText.ToUpper());
    }
  }
}
