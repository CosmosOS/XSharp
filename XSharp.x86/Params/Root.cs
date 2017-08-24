using System;
using System.Collections.Generic;
using System.Text;

namespace XSharp.x86.Params {
    public class Root : Param {
        public override bool IsMatch(object aValue) {
            return true;
        }
    }
}
