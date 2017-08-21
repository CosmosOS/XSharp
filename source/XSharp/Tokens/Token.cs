using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;

namespace XSharp.Tokens {
  public abstract class Token {
    protected List<Token> Tokens = new List<Token>();
    protected Action<List<CodePoint>> Emitter;
    protected Parsers.Parser Parser;

    protected abstract bool IsMatch(object aValue);

    protected void AddPattern(Action<List<CodePoint>> aEmitter, params Type[] aTokenTypes) {
      var xToken = this;
      foreach (var xType in aTokenTypes) {
        xToken = xToken.AddToken(xType);
      }

      if (xToken.Tokens.Count > 0) {
        throw new Exception("Cannot add emitter to a token which has subtokens.");
      }
      xToken.Emitter = aEmitter;
    }
    protected Token AddToken(Type aTokenType) {
      var xToken = Tokens.SingleOrDefault(q => q.GetType() == aTokenType);

      if (xToken == null) {
        if (Emitter != null) {
          throw new Exception("Cannot add subtokens to a token which has an emitter.");
        }
        xToken = (Token)Activator.CreateInstance(aTokenType);
        Tokens.Add(xToken);
      }

      return xToken;
    }

    public CodePoint Next(string aText, ref int rStart) {
      object xValue = null;

      int xThisStart = -1;
      for (xThisStart = rStart; xThisStart < aText.Length; xThisStart++) {
        if (char.IsWhiteSpace(aText[xThisStart]) == false) {
          break;
        }
      }
      if (xThisStart == aText.Length) {
        // All whitespace. Should never happen wtih our .TrimEnd(), but just in case.
        throw new Exception("No token text found on line.\r\n" + aText);
      }

      // Find which parser claims it.
      //
      // Yes, this looping is slow with all the calls. But for our current
      // needs its fast enough and worth the expansion.
      // Any optimazations should keep the basic design.
      // TODO - This can scan parsers more than once. Need to optimize this.
      rStart = xThisStart;
      foreach (var xToken in Tokens) {
        xValue = xToken.Parser.Parse(aText, ref rStart);
        if (xValue != null) {
          break;
        }
      }
      if (xValue == null) {
        throw new Exception("No matching parser found on line.\r\n" + aText);
      }

      foreach (var xToken in Tokens) {
        if (xToken.IsMatch(xValue)) {
          if (rStart == aText.Length) {
            if (xToken.Tokens != null) {
              throw new Exception("Incomplete line. Tokens exist beyond end of text.\r\n" + aText);
            } else if (xToken.Emitter == null) {
              throw new Exception("No emitter found for final token.\r\n" + aText);
            }
          } else if (xToken.Tokens == null) {
            throw new Exception("Text exists beyond end of recognized line.\r\n" + aText);
          }
          return new CodePoint(aText, xThisStart, rStart - 1, xToken, xValue);
        }
      }
      throw new Exception("No matching token found on line.\r\n" + aText);
    }
  }
}
