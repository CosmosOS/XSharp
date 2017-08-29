using System;
using System.Collections.Generic;
using System.Text;

namespace Spruce.Tokens {
    public class All : Token {
        public override object Parse(string aText, ref int rStart) {
            string xResult = aText.Substring(rStart);
            rStart = aText.Length;
            return xResult;
        }

        protected override bool Check(string aText) {
            return true;
        }
    }
}
