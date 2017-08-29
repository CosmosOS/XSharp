using System;
using System.Collections.Generic;
using System.Text;

namespace Spruce.Tokens {
    public class AlphaNumList : StringList {
        protected AlphaNumList(string aText, ResultFormat aResultFormat = ResultFormat.Normalize) : this(new[] { aText }, aResultFormat) { }

        protected AlphaNumList(string[] aList, ResultFormat aResultFormat = ResultFormat.Normalize) : base(aList, Chars.AlphaNum, aResultFormat) { }
    }
}
