using System;
using System.Collections.Generic;
using System.Text;
using XSharp.Values;

namespace XSharp.Tokens {
  public class Assignment : Token {
    public Assignment() {
      //Parser = new Parsers.Symbols();
    }

    protected override bool IsMatch(object aValue) {
      if (aValue is string) {
        return (string) aValue == "=";
      }
      return false;
    }
  }
}
