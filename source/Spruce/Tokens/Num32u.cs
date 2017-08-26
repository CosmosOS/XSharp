using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace Spruce.Tokens {
    public class Num32u : Num {
        protected override object Check(string aText) {
            return UInt32.Parse(aText);
        }
    }
}
