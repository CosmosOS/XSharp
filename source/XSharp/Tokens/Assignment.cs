using System;
using System.Collections.Generic;
using System.Text;

namespace XSharp.Tokens {
  public class Assignment : Token {
    public Assignment() {
      Parser = Parsers.Parsers.Operator;
    }

    protected override bool IsMatch(object aValue) {
      if (aValue is string) {
        return (string)aValue == "=";
      }
      return false;
    }
  }
}
