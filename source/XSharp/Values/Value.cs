using System;
using System.Collections.Generic;
using System.Text;

namespace XSharp.Values {
  public class Value {
    public readonly string Text;
    public readonly Tokens.Token Token;

    public Value(string aText, Tokens.Token aToken) {
      Text = aText;
      Token = aToken;
    }
  }
}
