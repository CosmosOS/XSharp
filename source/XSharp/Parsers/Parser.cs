using System;
using System.Collections.Generic;
using System.Text;

namespace XSharp.Parsers {
  public abstract class Parser {
    public static class Chars {
      public static readonly string Alpha;
      public static readonly string AlphaUpper = "ABCDEFGHIJKLMNOPQRTSUVWXYZ";
      public static readonly string AlphaLower;
      public static readonly string Number = "0123456789";
      public static readonly string AlphaNum;
      public static readonly string Symbol = "`!@#$%^&()+-*/=,.:{}[]";

      static Chars() {
        AlphaLower = AlphaUpper.ToLower();
        Alpha = AlphaUpper + AlphaLower;
        AlphaNum = Alpha + AlphaNum;
      }
    }

    public abstract Values.Value Parse(string aText, ref int rStart);
  }
}
