using System;
using System.Collections.Generic;
using System.Text;
using Parsers = Spruce.Parsers;

namespace Spruce.Tokens {
  public class Number32u : TypedToken<UInt32> {
    public Number32u() : base(Parsers.Parsers.Number64u) { }

    protected override bool IsMatch(ref UInt32 rValue) {
      return true;
    }
  }
}
