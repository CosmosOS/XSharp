using System;
using System.Collections.Generic;
using System.Text;

namespace Spruce.Tokens {
    public class Num08u : Num<byte> {
        //public class Num08u : Num {
        //    public override object Parse(string aText, ref int rStart) {
        //        string xVal = ParseToString(aText, ref rStart);
        //        if (xVal != null) {
        //            return byte.Parse(xVal);
        //        }
        //        return null;
        //    }
        //}        //protected override bool IsMatch(ref byte rValue) {
        //    return true;
        //}
        public override object Parse(string aText, ref int rStart) {
            throw new NotImplementedException();
        }
    }
}
