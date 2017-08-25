using System;
using System.Collections.Generic;
using System.Text;

namespace Spruce.Tokens {
    public class Num16u : Num<UInt16> {
        //public class Num16u : Num {
        //    public override object Parse(string aText, ref int rStart) {
        //        string xVal = ParseToString(aText, ref rStart);
        //        if (xVal != null) {
        //            return UInt16.Parse(xVal);
        //        }
        //        return null;
        //    }
        //}        //protected override bool IsMatch(ref UInt16 rValue) {
        //    return true;
        //}
        public override object Parse(string aText, ref int rStart) {
            throw new NotImplementedException();
        }
    }
}
