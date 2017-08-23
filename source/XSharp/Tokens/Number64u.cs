using System;
using System.Collections.Generic;
using System.Text;

namespace XSharp.Tokens {
  public class Number64u : Token {
    public Number64u() : base(Parsers.Parsers.Number64u) { }

    protected override object IsMatch(object aValue) {
      if (aValue is UInt64) {
        return aValue;
      }
      return null;
    }
  }
}
