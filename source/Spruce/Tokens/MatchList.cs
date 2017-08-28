using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace Spruce.Tokens {
    public class MatchList : Token {
        public enum ResultFormat {
            NoChange, Upper, Lower, MatchSource
        }
        protected ResultFormat FormatResult = ResultFormat.MatchSource;
        protected readonly string[] mList;
        protected StringComparison mCompare;

        protected MatchList(string aText, string aNoobChars = "", bool aIgnoreCase = true) : this(new[] { aText }, aNoobChars, aIgnoreCase) { }
        protected MatchList(string[] aList, string aNoobChars = "", bool aIgnoreCase = true) {
            mList = aList;
            mMaxLength = aList.Max(q => q.Length);
            mCompare = aIgnoreCase ? StringComparison.InvariantCultureIgnoreCase : StringComparison.InvariantCulture;

            // Can optimize based on mMatchCase but user can set
            // after ctor so need to account or update when prop changes.
            // Not critical as Check has final call, but would reduce size of Chars and
            // give a slight perf boost.
            BuildChars(aList, aNoobChars, aIgnoreCase);
        }

        public override object Check(string aText) {
            string xText = Array.Find(mList, q => q.Equals(aText, mCompare));

            if (xText == null) {
                return null;
            } else if (FormatResult == ResultFormat.NoChange) {
                return xText;
            } else if (FormatResult == ResultFormat.Upper) {
                return xText.ToUpper();
            } else if (FormatResult == ResultFormat.Lower) {
                return xText.ToLower();
            } else if (FormatResult == ResultFormat.MatchSource) {
                return xText;
            } else {
                throw new Exception("Unknown ResultFormat");
            }
        }
    }
}
