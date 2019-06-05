using System;
using System.Globalization;

namespace XSharp.Tokens
{
    public abstract class Num : Spruce.Tokens.Num {
        protected Func<string, NumberStyles, object> mParse;

        protected Num() : base(Chars.ExtraHexDigit, "$") {
        }

        protected override object Transform(string aText) {
            if (aText[0] == '$') {
                return mParse(aText.Substring(1), NumberStyles.HexNumber);
            }
            return mParse(aText, NumberStyles.Integer);
        }
    }

    public class Int08u : Num {
        public Int08u() {
            mParse = (aText, aStyle) => byte.Parse(aText, aStyle);
        }
    }

    public class Int16u : Num {
        public Int16u() {
            mParse = (aText, aStyle) => UInt16.Parse(aText, aStyle);
        }
    }

    public class Int32u : Num {
        public Int32u() {
            mParse = (aText, aStyle) => UInt32.Parse(aText, aStyle);
        }
    }
}
