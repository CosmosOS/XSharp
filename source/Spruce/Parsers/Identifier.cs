using System;
using System.Collections.Generic;
using System.Text;

namespace Spruce.Parsers {
    public class Identifier : Parser {
        protected string mFirstChars;
        protected string mChars;
        protected bool mUpperResult = false;

        public Identifier(bool aUpperResult = false) {
            mUpperResult = aUpperResult;

            mFirstChars = Parser.CharSets.Alpha + "_";
            mChars = mFirstChars + Parser.CharSets.Number;
        }

        public override object Parse(string aText, ref int rStart) {
            if (mFirstChars.IndexOf(aText[rStart]) == -1) {
                return null;
            }

            int i;
            for (i = rStart + 1; i < aText.Length; i++) {
                if (mChars.IndexOf(aText[i]) == -1) {
                    break;
                }
            }

            string xText = aText.Substring(rStart, i - rStart);
            if (mUpperResult) {
                xText = xText.ToUpper();
            }
            rStart = i;
            return xText;
        }
    }
}
