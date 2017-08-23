using System;
using System.Collections.Generic;
using System.Text;

namespace Spruce.Parsers {
  public abstract class Parser {
    public static class CharSets {
      public static readonly string Alpha;
      public static readonly string AlphaUpper = "ABCDEFGHIJKLMNOPQRTSUVWXYZ";
      public static readonly string AlphaLower;
      public static readonly string Number = "0123456789";
      public static readonly string AlphaNum;

      static CharSets() {
        AlphaLower = AlphaUpper.ToLower();
        Alpha = AlphaUpper + AlphaLower;
        AlphaNum = Alpha + AlphaNum;
      }
    }

    // Do not store any state in this class. It is
    // used from different places at once and only exists
    // to allow overrides since .NET types have no VMT.
    public abstract object Parse(string aText, ref int rStart);
  }
}
