using System;
using System.Collections.Generic;
using System.Text;

namespace Spruce.Tokens {
    public class Identifier : Token {
        protected string[] mList;
        protected bool mMatchCase;
        protected bool mUpperResult;

        protected Identifier(string aText, bool aMatchCase = false, bool aUpperResult = true) : this(new string[] {aText}, aMatchCase, aUpperResult) { }
        protected Identifier(string[] aList, bool aMatchCase = false, bool aUpperResult = true) {
            mList = aList;
            mMatchCase = aMatchCase;
            mUpperResult = aUpperResult;
        }

        //protected override bool IsMatch(ref string rValue) {
        //    return rValue == mText;
        //}
        public override object Parse(string aText, ref int rStart) {
            throw new NotImplementedException();
        }
    }
}
