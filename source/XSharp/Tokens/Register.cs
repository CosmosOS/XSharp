using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XSharp.Tokens {
  public class Register : RegisterBase {
    protected override bool IsMatch(object aValue) {
      // Probably can find a faster lookup method, but given that the lists are small hashing or other
      // method might be longer.
      if (aValue is string == false) {
        return false;
      }

      string xText = ((string) aValue).ToUpper();
        if (Register32.Names.Contains(xText)) {
          Size = 32;
        } else if (Register16.Names.Contains(xText)) {
          Size = 16;
        } else if (Register08.Names.Contains(xText)) {
          Size = 8;
        } else {
          return false;
        }
        return true;
    }
  }
}
