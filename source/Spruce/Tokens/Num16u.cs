using System;
using System.Collections.Generic;
using System.Text;

namespace Spruce.Tokens {
    public class Num16u : Num<UInt16> {
        //protected override bool IsMatch(ref UInt16 rValue) {
        //    return true;
        //}
        public override object Parse(string aText, ref int rStart) {
            throw new NotImplementedException();
        }
    }
}
