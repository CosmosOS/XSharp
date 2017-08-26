using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Spruce.Tokens {
    // Do not store any parse state in this class. It is
    // used from different places at once.
    public abstract class Token {
        public static class Chars {
            public static readonly string Alpha;
            public static readonly string Digit = "0123456789";
            public static readonly string AlphaUpper = "ABCDEFGHIJKLMNOPQRTSUVWXYZ";
            public static readonly string AlphaLower;
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

        // Used by default parse method
        protected int mMaxLength;
        protected string mFirstChars;
        protected string mChars;

        protected void SetChars(string aChars, string aFirstChars = null) {
            mChars = aChars;
            mFirstChars = aFirstChars ?? aChars;
        }

        protected void BuildChars(string[] aList) {
            void AddChar(StringBuilder aSB, char aChar) {
                if (!aSB.ToString().Contains(aChar)) {
                    // Convert to lower, simplest way as we convert to upper later
                    aChar = char.ToLowerInvariant(aChar);
                    aSB.Append(aChar);
                    char xCharUp = char.ToUpperInvariant(aChar);
                    if (xCharUp != aChar) {
                        aSB.Append(xCharUp);
                    }
                }
            }

            var xChars = new StringBuilder();
            var xFirstChars = new StringBuilder();
            foreach (var xString in aList) {
                if (string.IsNullOrEmpty(xString)) {
                    throw new Exception("Empty or null strings not permitted.");
                }
                AddChar(xFirstChars, xString[0]);
                if (xString.Length > 1) {
                    foreach (char xChar in xString.Substring(1)) {
                        AddChar(xChars, xChar);
                    }
                }
            }
            SetChars(xChars.ToString(), xFirstChars.ToString());
        }

        protected Token(string aChars = null, string aFirstChars = null) {
            SetChars(aChars, aFirstChars);
        }

        protected virtual bool CheckChar(int aLocalPos, char aChar) {
            if (aLocalPos == 0) {
                return mFirstChars.IndexOf(aChar) >= 0;
            }
            return mChars.IndexOf(aChar) >= 0;
        }

        protected abstract object Check(string aText);
        protected virtual object Parse(string aText, ref int rStart) {
            if (CheckChar(0, aText[rStart]) == false) {
                return null;
            }

            int i;
            for (i = 1; i < aText.Length - rStart; i++) {
                if (i > mMaxLength) {
                    // Exceeded max length, cant be what we are looking for.
                    return null;
                }
                if (CheckChar(i, aText[rStart + i]) == false) {
                    break;
                }
            }

            object xResult = Check(aText.Substring(rStart, i));
            if (xResult != null) {
                rStart += i;
            }
            return xResult;
        }

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
