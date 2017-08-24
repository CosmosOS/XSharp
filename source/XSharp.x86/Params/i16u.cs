using System;
using System.Collections.Generic;
using System.Text;

namespace XSharp.x86.Params {
    public class i16u : Param {
        public override bool IsMatch(object aValue) {
            return aValue is UInt16;
        }
    }
}
