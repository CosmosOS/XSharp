using System;
using System.Collections.Generic;
using System.Text;

namespace XSharp.Tokens {
  public abstract class Token {
    protected List<Token> Tokens = new List<Token>();

    public Token Next() {
      return null;
    }
  }
}
