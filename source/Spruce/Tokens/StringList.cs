using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace Spruce.Tokens {
    public class StringList : ObjectList<string> {
        // Normalize is case insensitive, but will reformat to match source (by returning the matching source)
        public enum ResultFormat { Normalize, MatchCase, IgnoreCase }
        protected readonly ResultFormat FormatResult = ResultFormat.Normalize;
        protected readonly string[] mList;
        protected readonly StringComparison mCompare;

        protected StringList(string aText, string aNoobChars = "", ResultFormat aResultFormat = ResultFormat.Normalize) : this(new[] { aText }, aNoobChars, aResultFormat) { }
        protected StringList(string[] aList, string aNoobChars = "", ResultFormat aResultFormat = ResultFormat.Normalize) {
            mList = aList;
            mMaxLength = aList.Max(q => q.Length);
            bool xIgnoreCase = aResultFormat != ResultFormat.MatchCase;
            mCompare = xIgnoreCase ? StringComparison.InvariantCultureIgnoreCase : StringComparison.InvariantCulture;
            BuildChars(aList, aNoobChars, xIgnoreCase);
        }

        protected override bool Check(string aText) {
            return Array.Exists(mList, q => q.Equals(aText, mCompare));
        }
    }
}
