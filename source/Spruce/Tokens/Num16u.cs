using System;
using System.Collections.Generic;
using System.Text;

namespace Spruce.Tokens {
    public class Num16u : Num<UInt16> {
        public Num16u() : base(Parsers.Parsers.Num16u) { }

        protected override bool IsMatch(ref UInt16 rValue) {
            return true;
        }
    }
}
