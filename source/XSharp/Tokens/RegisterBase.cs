using System;
using System.Collections.Generic;
using System.Text;

namespace XSharp.Tokens {
  public abstract class RegisterBase : Token {
    public int Size { get; protected set; }

    protected RegisterBase() {
      Parser = Parsers.Parsers.Identifiers;
    }
  }
}
