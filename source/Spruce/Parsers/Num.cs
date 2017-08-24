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
  }
}
