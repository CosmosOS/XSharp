using System;
using System.Collections.Generic;
using System.Text;

namespace Spruce.Tokens {
    public class AlphaNumList : MatchList {
        protected AlphaNumList(string aText, bool aIgnoreCase = true) : this(new[] { aText }, aIgnoreCase) { }

        protected AlphaNumList(string[] aList, bool aIgnoreCase = true) : base(aList, Chars.AlphaNum, aIgnoreCase) { }
    }
}
