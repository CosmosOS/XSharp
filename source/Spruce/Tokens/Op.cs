using System;
using System.Collections.Generic;
using System.Text;
using Parsers = Spruce.Parsers;

namespace Spruce.Tokens {
    public class Op : Token {
        protected string mText;

        protected Op(string aText) {
            mText = aText;
        }

        //protected override bool IsMatch(ref string rValue) {
        //  return rValue == mText;
        //}
        public override object Parse(string aText, ref int rStart) {
            throw new NotImplementedException();
        }
    }
}
