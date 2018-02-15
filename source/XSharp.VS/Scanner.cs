using System;
using System.Diagnostics.CodeAnalysis;
using Microsoft.VisualStudio.TextManager.Interop;
using MVSP = Microsoft.VisualStudio.Package;

namespace XSharp.VS {
  [SuppressMessage("Performance", "CA1810:Initialize reference type static fields inline")]
  internal class Scanner : MVSP.IScanner {
    struct TokenData {
      public MVSP.TokenType Type;
      public MVSP.TokenColor Color;
    }

    IVsTextBuffer mBuffer;
    XSharp.Parser mParser;
    int mTokenIdx;
    static TokenData[] mTokenMap;

    static Scanner() {
      int xEnumMax = Enum.GetValues(typeof(TokenType)).GetUpperBound(0);
      mTokenMap = new TokenData[xEnumMax + 1];

      // Set Default values
      foreach (int i in Enum.GetValues(typeof(TokenType))) {
        mTokenMap[i].Type = MVSP.TokenType.Unknown;
        mTokenMap[i].Color = MVSP.TokenColor.Text;
      }

      mTokenMap[(int)TokenType.Line_Comment] = new TokenData { Type = MVSP.TokenType.LineComment, Color = MVSP.TokenColor.Comment };
      mTokenMap[(int)TokenType.Line_LiteralAsm] = new TokenData { Type = MVSP.TokenType.Literal, Color = MVSP.TokenColor.String };
      mTokenMap[(int)TokenType.AlphaNum] = new TokenData { Type = MVSP.TokenType.Identifier, Color = MVSP.TokenColor.Identifier };
      mTokenMap[(int)TokenType.ValueInt] = new TokenData { Type = MVSP.TokenType.Literal, Color = MVSP.TokenColor.Number };

      var xKeyword = new TokenData { Type = MVSP.TokenType.Keyword, Color = MVSP.TokenColor.Keyword };
      mTokenMap[(int)TokenType.Register] = xKeyword;
      mTokenMap[(int)TokenType.Keyword] = xKeyword;

      mTokenMap[(int)TokenType.Delimiter] = new TokenData { Type = MVSP.TokenType.Delimiter, Color = MVSP.TokenColor.Text };
      mTokenMap[(int)TokenType.Operator] = new TokenData { Type = MVSP.TokenType.Operator, Color = MVSP.TokenColor.Text };
      mTokenMap[(int)TokenType.WhiteSpace] = new TokenData { Type = MVSP.TokenType.WhiteSpace, Color = MVSP.TokenColor.Text };
      mTokenMap[(int)TokenType.Unknown] = new TokenData { Type = MVSP.TokenType.Unknown, Color = MVSP.TokenColor.Text };
    }

    public Scanner(IVsTextBuffer aBuffer) {
      mBuffer = aBuffer;
    }

    // State argument: http://social.msdn.microsoft.com/Forums/en-US/vsx/thread/38939d76-6f8b-473f-9ee1-fc3ae7b59cce
    bool MVSP.IScanner.ScanTokenAndProvideInfoAboutIt(MVSP.TokenInfo aTokenInfo, ref int aState) {
      if (mTokenIdx == mParser.Tokens.Count) {
        return false;
      }

      var xToken = mParser.Tokens[mTokenIdx];
      mTokenIdx++;

      aTokenInfo.Token = (int)xToken.Type;
      aTokenInfo.StartIndex = xToken.SrcPosStart;
      aTokenInfo.EndIndex = xToken.SrcPosEnd;

      var xTokenData = mTokenMap[(int)xToken.Type];
      aTokenInfo.Type = xTokenData.Type;
      aTokenInfo.Color = xTokenData.Color;

      return true;
    }

    void MVSP.IScanner.SetSource(string aSource, int aOffset) {
      mTokenIdx = 0;
      mParser = new XSharp.Parser(aSource, true, false);
    }
  }
}
