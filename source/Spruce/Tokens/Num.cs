using System;
using System.Collections.Generic;
using System.Text;

namespace Spruce.Tokens {
    public abstract class Num : Token {
        protected Num() : base(Chars.Digit) { }
    }
}
