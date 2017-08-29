using System;
using System.Collections.Generic;
using System.Text;

namespace Spruce.Tokens {
    public class AlphaNumList : StringList {
        protected AlphaNumList(string aText, string aNoobChars = "", bool aIgnoreCase = true) : this(new[] { aText }, aNoobChars, aIgnoreCase) { }
        protected AlphaNumList(string[] aList, string aNoobChars = "", bool aIgnoreCase = true) : base(aList, aNoobChars, aIgnoreCase) { }
    }
}
