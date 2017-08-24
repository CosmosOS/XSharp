using System;
using System.Collections.Generic;
using System.Text;

namespace Spruce.Parsers {
    public class Num16u : Num {
        public override object Parse(string aText, ref int rStart) {
            string xVal = ParseToString(aText, ref rStart);
            if (xVal != null) {
                return UInt16.Parse(xVal);
            }
            return null;
        }
    }
}
