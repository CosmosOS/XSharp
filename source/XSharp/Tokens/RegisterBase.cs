using System;
using System.Collections.Generic;
using System.Text;

namespace XSharp.Tokens {
  public abstract class RegisterBase : Token {
    public int Size { get; protected set; }

    public RegisterBase() {
      Parser = Parsers.Parsers.Identifier;
    }
  }
}
