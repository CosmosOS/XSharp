using System;
using System.Collections.Generic;
using System.Text;

namespace XSharp.Tokens {
  public class Op : TypedToken<string> {
    protected string mText;

    protected Op(string aText) : base(Parsers.Parsers.Operator) {
      mText = aText;
    }

    protected override object IsMatch(string aValue) {
      if (aValue == mText) {
        return mText;
      }
      return null;
    }
  }
}
