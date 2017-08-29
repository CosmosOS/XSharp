using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Spruce.Attribs;

namespace Spruce.Tokens {

    // Do not store any parse state in this class. It is
    // used from different places at once.
    public abstract class Token {

        public static class Chars {
            public static readonly string Alpha;
            public static readonly string Digit = "0123456789";
            public static readonly string ExtraHexDigit = "ABCDEFabcdef";
            public static readonly string AlphaUpper = "ABCDEFGHIJKLMNOPQRTSUVWXYZ";
            public static readonly string AlphaLower;
            public static readonly string AlphaNum;

            static Chars() {
                AlphaLower = AlphaUpper.ToLower();
                Alpha = AlphaUpper + AlphaLower;
                AlphaNum = Alpha + Digit;
            }
        }

        public delegate void Action(List<CodePoint> aPoints);
        protected List<Token> mChildren = new List<Token>();
        public Action Emitter;

        protected int mMaxLength;
        protected readonly bool mIgnoreCase;
        protected string mFirstChars;
        protected string mChars;

        protected void SetChars(string aChars, string aFirstChars = null) {
            mChars = aChars;
            mFirstChars = aFirstChars ?? aChars;
        }

        // NOOB = Not Out Of Bound Chars - ie complete set of possible (even if not used) chars. Used for border detection.
        // mIgnoreCase must be set before calling BuildChars
        protected void BuildChars(string[] aList, string aNoobChars = "") {
            void AddChar(StringBuilder aSB, char aChar) {
                if (!aSB.ToString().Contains(aChar)) {
                    if (mIgnoreCase) {
                        // Convert to upper, simplest way as we convert to lower later
                        aChar = char.ToUpperInvariant(aChar);
                        aSB.Append(aChar);
                        char xCharUp = char.ToLowerInvariant(aChar);
                        // If its not alpha, won't be different so don't add twice.
                        if (xCharUp != aChar) {
                            aSB.Append(xCharUp);
                        }
                    } else {
                        aSB.Append(aChar);
                    }
                }
            }

            var xChars = new StringBuilder();
            var xFirstChars = new StringBuilder();
            foreach (var xString in aList) {
                if (string.IsNullOrEmpty(xString)) {
                    throw new Exception("Empty or null strings not permitted.");
                }

                // Don't add NOOB chars to first char, not needed.
                AddChar(xFirstChars, xString[0]);

                if (xString.Length > 1) {
                    foreach (char xChar in xString.Substring(1) + aNoobChars) {
                        AddChar(xChars, xChar);
                    }
                }
            }
            SetChars(xChars.ToString(), xFirstChars.ToString());
        }

        protected Token(bool aIgnoreCase = false) {
            mIgnoreCase = aIgnoreCase;
        }
        protected Token(string aChars, string aFirstChars = null, bool aIgnoreCase = false) : this(aIgnoreCase) {
            mIgnoreCase = aIgnoreCase;
            SetChars(aChars, aFirstChars);
        }

        protected virtual bool CheckChar(int aLocalPos, char aChar) {
            if (aLocalPos == 0) {
                return mFirstChars.IndexOf(aChar) > -1;
            }
            return mChars.IndexOf(aChar) > -1;
        }

        // Default stages - can be overridden individually or whole.
        // 1) Parse get a string of chars that potentially match what we need.
        // 2) Check - See if it matches exactly or not. May or may not be a simple comparison.
        //            MUST be overridden unless Parse is also overridden.
        // 3) Transform - Transform into the desired result object. May remain a string (default)
        //    or be converted int a number or other full object type.
        //
        // TODO Can seperate the NOOB chars and short circuit earlier but may not be worth the additional overhead.
        public virtual object Parse(string aText, ref int rStart) {
            // Check first char
            if (CheckChar(0, aText[rStart]) == false) {
                return null;
            }

            // Check chars 2+
            int xLength = aText.Length - rStart;
            if (mMaxLength > 0) {
                // Short circuit if maxlength, but only if its less than actual length avail
                // +1 because we need to look one past to avoid conflicts like CL vs CLEAR
                xLength = Math.Min(xLength, mMaxLength + 1);
            }
            // Not 0 - We already did 0 at start of method.
            int i = 1;
            while (i < xLength && CheckChar(i, aText[rStart + i])) {
                i++;
            }

            string xText = aText.Substring(rStart, i);
            if (mIgnoreCase) xText = xText.ToUpper();
            if (Check(xText)) {
                rStart += i;
                return Transform(xText);
            }
            return null;
        }
        // NotImpl instead of abstract. Parse calls Check, but if Parse is overriden
        // then check is often not needed and this makes it so such descendants
        // dont need to override something they don't use anyway.
        protected virtual bool Check(string aText) {
            throw new NotImplementedException();
        }
        protected virtual object Transform(string aText) {
            return aText;
        }

        protected void AddEmitter(Action aEmitter, params Type[] aTokenTypes) {
            var xToken = this;
            for (int i = 0; i < aTokenTypes.Length; i++) {
                var xType = aTokenTypes[i];
                if (xType.IsDefined(typeof(GroupToken), false)) {
                    // Group token, need to split tree
                    var xGroupAttrib = xType.GetCustomAttribute<GroupToken>();
                    // New array with each group token + rest that follow.
                    // Copy rest but leave first slot open.
                    var xTokenTypes = new Type[aTokenTypes.Length - i];
                    for (int j = 1; j < aTokenTypes.Length - i; j++) {
                        xTokenTypes[j] = aTokenTypes[i + j];
                    }
                    foreach (var xGroupType in xGroupAttrib.TokenTypes) {
                        xTokenTypes[0] = xGroupType;
                        xToken.AddEmitter(aEmitter, xTokenTypes);
                    }
                    return;
                } else {
                    // Single token
                    xToken = xToken.AddToken(xType);
                }
            }
            xToken.Emitter = aEmitter;
        }

        protected Token AddToken(Type aTokenType) {
            var xToken = mChildren.SingleOrDefault(q => q.GetType() == aTokenType);
            if (xToken == null) {
                xToken = (Token)Activator.CreateInstance(aTokenType);
                mChildren.Add(xToken);
            }
            return xToken;
        }

        protected void SkipWhiteSpace(string aText, ref int rStart) {
            for (; rStart < aText.Length; rStart++) {
                if (char.IsWhiteSpace(aText[rStart]) == false) {
                    break;
                }
            }
            if (rStart == aText.Length) {
                // All whitespace. Should never happen wtih our .TrimEnd(), but just in case.
                throw new Exception("End of line reached.");
            }
        }

        public CodePoint Next(string aText, ref int rStart) {
            if (mChildren.Count == 0) {
                throw new Exception("No tokens to scan.");
            }

            SkipWhiteSpace(aText, ref rStart);
            int xThisStart = rStart;
            foreach (var xToken in mChildren) {
                var xValue = xToken.Parse(aText, ref rStart);
                if (xValue != null) {
                    return new CodePoint(aText, xThisStart, rStart - 1, xToken, xValue);
                }
            }
            throw new Exception("No matching token found on line.");
        }
    }
}
