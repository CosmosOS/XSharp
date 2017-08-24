using System;
using System.Collections.Generic;
using System.Text;
using Parsers = Spruce.Parsers;

namespace Spruce.Tokens {
  public class Num32u : TypedToken<UInt32> {
    public Num32u() : base(Parsers.Parsers.Number64u) { }

    protected override bool IsMatch(ref UInt32 rValue) {
      return true;
    }
  }
}
