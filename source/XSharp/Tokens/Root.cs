using System;
using System.Collections.Generic;
using System.Text;

namespace XSharp.Tokens {
  public class Root : Token {
    public Root() {
      Tokens.Add(new Register());
    }
  }
}
