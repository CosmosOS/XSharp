using System;
using System.Collections.Generic;
using System.Text;

namespace Spruce.Tokens
{
    public class AlphaNum : Token
    {
        // First char cannot be digit
        public AlphaNum() : base(Chars.AlphaNum, Chars.Alpha) { }

        protected override object Check(string aText)
        {
            return aText;
        }
    }
}
