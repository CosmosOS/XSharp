using System;
using System.Collections.Generic;
using System.Text;

namespace XSharp.Tokens {
  public abstract class Token {
    protected List<Token> Tokens = new List<Token>();
    protected Parsers.Parser Parser;

    protected abstract bool IsMatch(object aValue);

    public CodePoint Next(string aText, ref int rStart) {
      int i;
      for (i = rStart; i < aText.Length; i++) {
        if (char.IsWhiteSpace(aText[i]) == false) {
          // Yes, this looping is slow with all the calls. But for our current
          // needs its fast enough and worth the expansion.
          // Any optimazations should keep the basic design.
          foreach (var xParser in Tokens) {
            // Find which parser claims it.
            // TODO - This can scan parsers more than once. Need to optimize this.
            var xValue = xParser.Parser.Parse(aText, ref i);
            if (xValue != null) {
              foreach (var xToken in Tokens) {
                if (xToken.IsMatch(xValue)) {
                  rStart = i;
                  return new CodePoint(aText, rStart, i - 1, xToken, xValue);
                }
              }
            }
          }
        }
      }
      return null;
    }
  }
}
