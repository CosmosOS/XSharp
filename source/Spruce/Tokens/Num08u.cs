using System;
using System.Collections.Generic;
using System.Text;

namespace Spruce.Tokens {
    public class Num08u : Num<byte> {
        public Num08u() : base(Parsers.Parsers.Num08u) { }

        protected override bool IsMatch(ref byte rValue) {
            return true;
        }
    }
}
