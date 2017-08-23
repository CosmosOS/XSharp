using System;
using System.Collections.Generic;
using System.Text;

namespace XSharp.Tokens {
  public class Op : Token {
    protected string mText;

    protected Op(string aText) : base(Parsers.Parsers.Operator) {
      mText = aText;
    }

    protected override object IsMatch(object aValue) {
      if (aValue is string && (string)aValue == mText) {
        return mText;
      }
      return null;
    }
  }
}
