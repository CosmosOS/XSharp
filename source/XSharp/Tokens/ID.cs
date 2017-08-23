using System;
using System.Collections.Generic;
using System.Text;

namespace XSharp.Tokens {
  public class ID : TypedToken<string> {
    protected string mText;

    public ID(string aText = null, bool aUpper = true) : base(aUpper ? Parsers.Parsers.IdentifierUpper : Parsers.Parsers.Identifier) {
      if (aText != null) {
        if (aUpper) {
          mText = aText.ToUpper();
        } else {
          mText = aText;
        }
      }
    }

    protected override bool IsMatch(ref string rValue) {
      return rValue == mText;
    }
  }
}
