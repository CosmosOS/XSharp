using System;
using System.Collections.Generic;
using System.Text;

namespace XSharp.Tokens {
  public class Assignment : Token {
    public Assignment() {
      mParser = Parsers.Parsers.Operator;
    }

    protected override object IsMatch(object aValue) {
      if (aValue is string && (string)aValue == "=") {
        return "=";
      }
      return null;
    }
  }
}
