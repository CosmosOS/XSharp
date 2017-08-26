using System;
using System.Collections.Generic;
using System.Text;

namespace Spruce.Tokens {
    public abstract class Num : Token {
        protected Num(string aChars = null, string aFirstChars = null) : base(aChars ?? Chars.Digit, aFirstChars) { }
    }

    public class Num08u : Num {
        protected override object Check(string aText) {
            return byte.Parse(aText);
        }
    }

    public class Num16u : Num {
        protected override object Check(string aText) {
            return UInt16.Parse(aText);
        }
    }

    public class Num32u : Num {
        protected override object Check(string aText) {
            return UInt32.Parse(aText);
        }
    }
}
