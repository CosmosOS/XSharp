using System.Collections.Generic;
using Microsoft.VisualStudio.TextManager.Interop;
using MVSP = Microsoft.VisualStudio.Package;

namespace XSharp.VS
{
    internal class Scanner : MVSP.IScanner
    {
        private struct TokenData
        {
            public MVSP.TokenType Type { get; }
            public MVSP.TokenColor Color { get; }

            public TokenData(MVSP.TokenType type, MVSP.TokenColor color)
            {
                Type = type;
                Color = color;
            }
        }

        private IVsTextBuffer _buffer;
        private XSharp.Parser _parser;
        private int _tokenIdx;

        private static Dictionary<TokenType, TokenData> TokenMap = new Dictionary<TokenType, TokenData>()
        {
            [TokenType.Line_Comment] = new TokenData(MVSP.TokenType.LineComment, MVSP.TokenColor.Comment),
            [TokenType.Line_LiteralAsm] = new TokenData(MVSP.TokenType.Literal, MVSP.TokenColor.String),
            [TokenType.AlphaNum] = new TokenData(MVSP.TokenType.Identifier, MVSP.TokenColor.Identifier),
            [TokenType.ValueInt] = new TokenData(MVSP.TokenType.Literal, MVSP.TokenColor.Number),

            [TokenType.Register] = new TokenData(MVSP.TokenType.Keyword, MVSP.TokenColor.Keyword),
            [TokenType.Keyword] = new TokenData(MVSP.TokenType.Keyword, MVSP.TokenColor.Keyword),

            [TokenType.Delimiter] = new TokenData(MVSP.TokenType.Delimiter, MVSP.TokenColor.Text),
            [TokenType.Operator] = new TokenData(MVSP.TokenType.Operator, MVSP.TokenColor.Text),
            [TokenType.WhiteSpace] = new TokenData(MVSP.TokenType.WhiteSpace, MVSP.TokenColor.Text),
            [TokenType.Unknown] = new TokenData(MVSP.TokenType.Unknown, MVSP.TokenColor.Text)
        };

        public Scanner(IVsTextBuffer aBuffer)
        {
            _buffer = aBuffer;
        }

        #region IScanner

        // State argument: http://social.msdn.microsoft.com/Forums/en-US/vsx/thread/38939d76-6f8b-473f-9ee1-fc3ae7b59cce
        public bool ScanTokenAndProvideInfoAboutIt(MVSP.TokenInfo tokenInfo, ref int state)
        {
            if (_tokenIdx == _parser.Tokens.Count)
            {
                return false;
            }

            var xToken = _parser.Tokens[_tokenIdx];
            _tokenIdx++;

            tokenInfo.Token = (int)xToken.Type;
            tokenInfo.StartIndex = xToken.SrcPosStart;
            tokenInfo.EndIndex = xToken.SrcPosEnd;

            if (TokenMap.TryGetValue(xToken.Type, out var xTokenData))
            {
                tokenInfo.Type = xTokenData.Type;
                tokenInfo.Color = xTokenData.Color;
            }
            else
            {
                tokenInfo.Type = MVSP.TokenType.Unknown;
                tokenInfo.Color = MVSP.TokenColor.Text;
            }

            return true;
        }

        public void SetSource(string source, int offset)
        {
            _tokenIdx = 0;
            _parser = new XSharp.Parser(source, true, false);
        }

        #endregion
    }
}
