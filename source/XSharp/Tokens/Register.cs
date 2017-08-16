using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XSharp.Tokens {
  public class Register : RegisterBase {
    protected override bool IsMatch(string aText) {
      // Probably can find a faster lookup method, but given that the lists are small hashing or other
      // method might be longer.
      if (Register32.Names.Contains(aText.ToUpper())) {
        Size = 32;
        return true;
      } else if (Register16.Names.Contains(aText.ToUpper())) {
        Size = 16;
        return true;
      } else if (Register08.Names.Contains(aText.ToUpper())) {
        Size = 8;
        return true;
      }
      return false;
    }
  }
}
