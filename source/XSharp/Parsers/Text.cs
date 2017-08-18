using System;
using System.Collections.Generic;
using System.Text;

namespace XSharp.Parsers {
  public class Text : Parser {
    protected static readonly string FirstChars;
    protected static readonly string Chars;

    static Text() {
      FirstChars = Parser.Chars.Alpha + "_";
      Chars = FirstChars + Parser.Chars.Number;
    }

    public override object Parse(string aText, ref int rStart) {
      if (FirstChars.IndexOf(aText[rStart]) == -1) {
        return null;
      }

      var xOut = new StringBuilder(aText[rStart]);
      int i;
      for (i = rStart + 1; i < aText.Length; i++) {
        if (Chars.IndexOf(aText[i]) == -1) {
          break;
        }
      }

      string xText = aText.Substring(rStart, i - rStart);
      rStart = i;
      return xText;
    }
  }
}
