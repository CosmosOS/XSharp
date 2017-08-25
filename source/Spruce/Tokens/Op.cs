using System;
using System.Collections.Generic;
using System.Text;

namespace Spruce.Tokens {
    public class Op : Token {
        protected string mText;

        protected Op(string aText) {
            mText = aText;
        }

        //public class Operator : Parser {
        //  protected static readonly string Chars;

        //  static Operator() {
        //    Chars = @"=+-*/\!@#$%^&";
        //  }

        //  public override object Parse(string aText, ref int rStart) {
        //    int i;
        //    for (i = rStart; i < aText.Length; i++) {
        //      if (Chars.IndexOf(aText[i]) == -1) {
        //        break;
        //      }
        //    }
        //    if (i == rStart) {
        //      return null;
        //    }

        //    string xText = aText.Substring(rStart, i - rStart);
        //    rStart = i;
        //    return xText;
        //  }
        //}        //protected override bool IsMatch(ref string rValue) {
        //  return rValue == mText;
        //}
        public override object Parse(string aText, ref int rStart) {
            throw new NotImplementedException();
        }
    }
}
