using System;
using System.Collections.Generic;
using System.Text;
using XSharp.Parsers;

namespace XSharp.Tokens {
  public class All : Token {
    public All() : base(Parsers.Parsers.All) { }

    protected override object IsMatch(object aValue) {
      if (aValue is string) {
        return aValue;
      }
      return null;
    }
  }
}
