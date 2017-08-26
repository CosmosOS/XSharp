using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace Spruce.Tokens {
    public abstract class Num : Token {
        protected Num(string aExtraChars = "", string aExtraFirstChars = "") : base(aExtraChars + Chars.Digit, aExtraFirstChars + Chars.Digit) { }
    }

    public class Num08u : Num {
        protected override object Check(string aText) {
            return byte.Parse(aText, NumberStyles.Integer);
        }
    }

    public class Num16u : Num {
        protected override object Check(string aText) {
            return UInt16.Parse(aText, NumberStyles.Integer);
        }
    }

    public class Num32u : Num {
        protected override object Check(string aText) {
            return UInt32.Parse(aText, NumberStyles.Integer);
        }
    }
}
