using System;
using System.Collections.Generic;
using System.Text;

namespace Spruce.Parsers {
  public abstract class Num : Parser {
      protected static readonly string FirstChars;
      protected static readonly string Chars;

      static Num() {
          Chars = CharSets.Number;
          // Hex, etc.. need to find current X# syntax
          FirstChars = "" + Chars;
      }

      protected string ParseToString(string aText, ref int rStart) {
          if (FirstChars.IndexOf(aText[rStart]) == -1) {
              return null;
          }

          int i;
          for (i = rStart + 1; i < aText.Length; i++) {
              if (Chars.IndexOf(aText[i]) == -1) {
                  break;
              }
          }

          string xResult = aText.Substring(rStart, i - rStart);
          rStart = i;
          return xResult;
      }
  }
}
