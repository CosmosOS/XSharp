using System;
using System.Collections.Generic;
using System.Text;

namespace Spruce.Tokens {
    public class Num32u : Num<UInt32> {
        //protected override bool IsMatch(ref UInt32 rValue) {
        //  return true;
        //}
        public override object Parse(string aText, ref int rStart) {
            throw new NotImplementedException();
        }
    }
}
