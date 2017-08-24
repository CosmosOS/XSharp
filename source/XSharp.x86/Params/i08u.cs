using System;
using System.Collections.Generic;
using System.Text;

namespace XSharp.x86.Params {
    public class i08u : Param {
        public override bool IsMatch(object aValue) {
            return aValue is byte;
        }
    }
}
