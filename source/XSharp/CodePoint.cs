using System;
using System.Collections.Generic;
using System.Text;

namespace XSharp {
  public class CodePoint {
    public readonly string FullText;
    public readonly int TextStart;
    public readonly int TextEnd;
    public readonly Tokens.Token Token;
    public readonly object Value;

    public CodePoint(string aFullText, int aTextStart, int aTextEnd, Tokens.Token aToken, object aValue) {
      FullText = aFullText;
      //
      TextStart = aTextStart;
      TextEnd = aTextEnd;
      //
      Token = aToken;
      Value = aValue;
    }
  }
}
