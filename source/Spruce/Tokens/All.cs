using System;
using System.Collections.Generic;
using System.Text;

namespace Spruce.Tokens {
    public class All : Token {
        protected override object Parse(string aText, ref int rStart) {
            string xResult = aText.Substring(rStart);
            rStart = aText.Length;
            return xResult;
        }

        protected override object Check(string aText) {
            return aText;
        }
    }
}
