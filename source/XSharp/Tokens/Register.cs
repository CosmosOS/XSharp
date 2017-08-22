using System;
using System.Collections.Generic;
using System.Text;

namespace XSharp.Tokens {
  public abstract class Register : Token {
    protected Register() {
      mParser = Parsers.Parsers.IdentifierUpper;
    }
  }
}
