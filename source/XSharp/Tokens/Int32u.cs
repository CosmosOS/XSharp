using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace XSharp.Tokens {
  public class Int32u : Spruce.Tokens.Num32u {
    protected override bool CheckChar(int aLocalPos, char aChar) {
      if (aLocalPos == 0) {
        if (aChar == '$') {
          return true;
        }
      } else if (Chars.ExtraHexDigit.IndexOf(aChar) > -1) {
        return true;
      }
      return base.CheckChar(aLocalPos, aChar);
    }

    protected override object Check(string aText) {
      if (aText[0] == '$') {
        return UInt32.Parse(aText.Substring(1), NumberStyles.HexNumber);
      }
      return base.Check(aText);
    }
  }
}
