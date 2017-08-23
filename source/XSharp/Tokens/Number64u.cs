using System;
using System.Collections.Generic;
using System.Text;

namespace XSharp.Tokens {
  public class Number64u : TypedToken<UInt64> {
    public Number64u() : base(Parsers.Parsers.Number64u) { }

    protected override object IsMatch(UInt64 aValue) {
      return aValue;
    }
  }
}
