using System;
using System.Collections.Generic;
using System.Text;

namespace XSharp.Tokens {
  public class Number64u : Token {
    public Number64u() {
      Parser = Parsers.Parsers.Number64u;
    }

    protected override bool IsMatch(object aValue) {
      return aValue is UInt64;
    }
  }
}
