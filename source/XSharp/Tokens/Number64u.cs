using System;
using System.Collections.Generic;
using System.Text;
using Parsers = Spruce.Parsers;

namespace XSharp.Tokens {
  public class Number64u : TypedToken<UInt64> {
    public Number64u() : base(Parsers.Parsers.Number64u) { }

    protected override bool IsMatch(ref UInt64 rValue) {
      return true;
    }
  }
}
