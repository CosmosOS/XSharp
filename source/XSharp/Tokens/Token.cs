using System;
using System.Collections.Generic;
using System.Text;

namespace XSharp.Tokens {
  public abstract class Token {
    protected List<Token> Tokens = new List<Token>();
    protected Parsers.Parser Parser;

    protected abstract bool IsMatch(string aText);

    public Token Next(string aText, ref int rStart) {
      for (int i = rStart; i < aText.Length; i++) {
        if (char.IsWhiteSpace(aText[i]) == false) {
          // Yes, this looping is slow with all the calls. But for our current
          // needs its fast enough and worth the expansion.
          // Any optimazations should keep the basic design.
          foreach (var xParser in Tokens) {
            // Find which parser claims it.
            var xValue = xParser.Parser.Parse(aText, ref i);
            if (xValue != null) {
              foreach (var xToken in Tokens) {
                // pass in Value itself
              }
            }
          }
        }
      }
      return null;
    }
  }
}
