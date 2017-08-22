using System;
using System.Collections.Generic;
using System.Text;

namespace XSharp.Tokens {
  public class Identifier : Token {
    protected string mText;

    public Identifier(string aText = null, bool aUpper = true) {
      if (aUpper) {
        mParser = Parsers.Parsers.IdentifierUpper;
      } else {
        mParser = Parsers.Parsers.Identifier;
      }

      if (aText != null) {
        if (aUpper) {
          mText = aText.ToUpper();
        } else {
          mText = aText;
        }
      }
    }

    protected override object IsMatch(object aValue) {
      if (aValue is string && (string)aValue == mText) {
        return mText;
      }
      return null;
    }
  }
}
