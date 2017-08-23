using System;
using System.Collections.Generic;
using System.Text;
using XSharp.Parsers;

namespace XSharp.Tokens {
  public class All : TypedToken<string> {
    public All() : base(Parsers.Parsers.All) { }

    protected override object IsMatch(string aValue) {
      return aValue;
    }
  }
}
