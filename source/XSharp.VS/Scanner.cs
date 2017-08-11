using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio;
using Microsoft.VisualStudio.Package;
using Microsoft.VisualStudio.TextManager.Interop;
using Microsoft.VisualStudio.OLE.Interop;
using XS = XSharp;

namespace XSharp.VS
{
  internal class Scanner : IScanner
  {
    struct TokenData
    {
      public TokenType Type;
      public TokenColor Color;
    }

    IVsTextBuffer mBuffer;
    XS.Parser mParser;
    int mTokenIdx;
    static TokenData[] mTokenMap;

    static Scanner()
    {
      int xEnumMax = Enum.GetValues(typeof(XS.TokenType)).GetUpperBound(0);
      mTokenMap = new TokenData[xEnumMax + 1];

      // Set Default values
      foreach (int i in Enum.GetValues(typeof(XS.TokenType)))
      {
        mTokenMap[i].Type = TokenType.Unknown;
        mTokenMap[i].Color = TokenColor.Text;
      }

      mTokenMap[(int)XS.TokenType.Comment] = new TokenData { Type = TokenType.LineComment, Color = TokenColor.Comment };
      mTokenMap[(int)XS.TokenType.LiteralAsm] = new TokenData { Type = TokenType.Literal, Color = TokenColor.String };
      mTokenMap[(int)XS.TokenType.AlphaNum] = new TokenData { Type = TokenType.Identifier, Color = TokenColor.Identifier };
      mTokenMap[(int)XS.TokenType.ValueInt] = new TokenData { Type = TokenType.Literal, Color = TokenColor.Number };

      var xKeyword = new TokenData { Type = TokenType.Keyword, Color = TokenColor.Keyword };
      mTokenMap[(int)XS.TokenType.Register] = xKeyword;
      mTokenMap[(int)XS.TokenType.Keyword] = xKeyword;

      mTokenMap[(int)XS.TokenType.Delimiter] = new TokenData { Type = TokenType.Delimiter, Color = TokenColor.Text };
      mTokenMap[(int)XS.TokenType.Operator] = new TokenData { Type = TokenType.Operator, Color = TokenColor.Text };
      mTokenMap[(int)XS.TokenType.WhiteSpace] = new TokenData { Type = TokenType.WhiteSpace, Color = TokenColor.Text };
      mTokenMap[(int)XS.TokenType.Unknown] = new TokenData { Type = TokenType.Unknown, Color = TokenColor.Text };
    }

    public Scanner(IVsTextBuffer aBuffer)
    {
      mBuffer = aBuffer;
    }

    // State argument: http://social.msdn.microsoft.com/Forums/en-US/vsx/thread/38939d76-6f8b-473f-9ee1-fc3ae7b59cce
    bool IScanner.ScanTokenAndProvideInfoAboutIt(TokenInfo aTokenInfo, ref int aState)
    {
      if (mTokenIdx == mParser.Tokens.Count)
      {
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

    void IScanner.SetSource(string aSource, int aOffset)
    {
      mTokenIdx = 0;
      mParser = new XS.Parser(aSource, aOffset, true, false);
    }
  }
}
