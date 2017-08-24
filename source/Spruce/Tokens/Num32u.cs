using System;
using System.Collections.Generic;
using System.Text;

namespace Spruce.Tokens {
  public class Num32u : Num<UInt32> {
    public Num32u() : base(Parsers.Parsers.Num32u) { }

    protected override bool IsMatch(ref UInt32 rValue) {
      return true;
    }
  }
}
