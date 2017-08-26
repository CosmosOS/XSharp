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

        protected bool mMatchCase {
            set => mCompare = value ? StringComparison.InvariantCulture : StringComparison.InvariantCultureIgnoreCase;
        }

        protected MatchList(string aText) : this(new[] { aText }) { }
        protected MatchList(string[] aList) {
            mList = aList;
            mMaxLength = aList.Max(q => q.Length);
            mMatchCase = false; // Calls setter to set default.
            BuildChars(aList);
        }

        protected override object Check(string aText) {
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
