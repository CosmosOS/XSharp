using System;
using System.Collections.Generic;
using System.Text;

namespace Spruce.Tokens {
    public class Num32u : Num<UInt32> {
        //public class Num32u : Num {
        //    public override object Parse(string aText, ref int rStart) {
        //        string xVal = ParseToString(aText, ref rStart);
        //        if (xVal != null) {
        //            return UInt32.Parse(xVal);
        //        }
        //        return null;
        //    }
        //}        //protected override bool IsMatch(ref UInt32 rValue) {
        //  return true;
        //}
        public override object Parse(string aText, ref int rStart) {
            throw new NotImplementedException();
        }
    }
}
