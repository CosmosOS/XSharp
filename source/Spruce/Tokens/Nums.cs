using System;
using System.Collections.Generic;
using System.Text;

namespace Spruce.Tokens {
    public abstract class Num : Token {
        protected Num() : base(Chars.Digit) { }
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
