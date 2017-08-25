using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using Parsers = Spruce.Parsers;

namespace Spruce.Tokens {
    // Do not store any parse state in this class. It is
    // used from different places at once.
    public abstract class Token {
        public static class Chars {
            public static readonly string Alpha;
            public static readonly string AlphaUpper = "ABCDEFGHIJKLMNOPQRTSUVWXYZ";
            public static readonly string AlphaLower;
            public static readonly string Number = "0123456789";
            public static readonly string AlphaNum;

            static Chars() {
                AlphaLower = AlphaUpper.ToLower();
                Alpha = AlphaUpper + AlphaLower;
                AlphaNum = Alpha + AlphaNum;
            }
        }

        public delegate void Action(List<CodePoint> aPoints);
        protected List<Token> mTokens = new List<Token>();
        public Action Emitter;

        public abstract object Parse(string aText, ref int rStart);

        protected void AddEmitter(Action aEmitter, params Type[] aTokenTypes) {
            var xToken = this;
            foreach (var xType in aTokenTypes) {
                xToken = xToken.AddToken(xType);
            }

            if (xToken.mTokens.Count > 0) {
                throw new Exception("Cannot add emitter to a token which has subtokens.");
            }
            xToken.Emitter = aEmitter;
        }
        protected Token AddToken(Type aTokenType) {
            var xToken = mTokens.SingleOrDefault(q => q.GetType() == aTokenType);

            if (xToken == null) {
                if (Emitter != null) {
                    throw new Exception("Cannot add subtokens to a token which has an emitter.");
                }
                xToken = (Token)Activator.CreateInstance(aTokenType);
                mTokens.Add(xToken);
            }

            return xToken;
        }

        public CodePoint Next(string aText, ref int rStart) {
            int xThisStart = -1;
            for (xThisStart = rStart; xThisStart < aText.Length; xThisStart++) {
                if (char.IsWhiteSpace(aText[xThisStart]) == false) {
                    break;
                }
            }
            if (xThisStart == aText.Length) {
                // All whitespace. Should never happen wtih our .TrimEnd(), but just in case.
                throw new Exception("End of line reached.");
            }

            rStart = xThisStart;
            foreach (var xToken in mTokens) {
                var xValue = xToken.Parse(aText, ref rStart);
                if (xValue != null) {
                    return new CodePoint(aText, xThisStart, rStart - 1, xToken, xValue);
                }
            }
            throw new Exception("No matching token found on line.");
        }
    }
}
