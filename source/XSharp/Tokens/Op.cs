using System;
using System.Collections.Generic;
using System.Text;
using Parsers = Spruce.Parsers;

namespace XSharp.Tokens {
  public class Op : TypedToken<string> {
    protected string mText;

    protected Op(string aText) : base(Parsers.Parsers.Operator) {
      mText = aText;
    }

    protected override bool IsMatch(ref string rValue) {
      return rValue == mText;
    }
  }
}
