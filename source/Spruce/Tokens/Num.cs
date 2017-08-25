using System;
using System.Collections.Generic;
using System.Text;

namespace Spruce.Tokens {
    public abstract class Num<T> : Token {
        //public abstract class Num : Parser {
        //    protected readonly string FirstChars;
        //    protected readonly string Chars;

        //    protected Num() {
        //        FirstChars = CharSets.Number;
        //        Chars = CharSets.Number;
        //    }

        //    protected string ParseToString(string aText, ref int rStart) {
        //        if (FirstChars.IndexOf(aText[rStart]) == -1) {
        //            return null;
        //        }

        //        int i;
        //        for (i = rStart + 1; i < aText.Length; i++) {
        //            if (Chars.IndexOf(aText[i]) == -1) {
        //                break;
        //            }
        //        }

        //        string xResult = aText.Substring(rStart, i - rStart);
        //        rStart = i;
        //        return xResult;
        //    }
        //}
    }
}
