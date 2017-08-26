using System;
using System.Collections.Generic;
using System.Text;

namespace Spruce.Tokens {
    public class Num08u : Num {
        protected override object Check(string aText) {
            return byte.Parse(aText);
        }
    }
}
