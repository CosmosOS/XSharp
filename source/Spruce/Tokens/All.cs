using System;
using System.Collections.Generic;
using System.Text;
using Parsers = Spruce.Parsers;

namespace Spruce.Tokens {
  public class All : TypedToken<string> {
    public All() : base(Parsers.Parsers.All) { }

    protected override bool IsMatch(ref string rValue) {
      return true;
    }
  }
}
