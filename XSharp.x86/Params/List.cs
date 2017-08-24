using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace XSharp.x86.Params {
    public class List : Param {
        protected readonly string[] mTexts;

        public List(string[] aTexts) {
            mTexts = aTexts;
        }

        public override bool IsMatch(object aValue) {
            if (aValue is string) {
                return mTexts.Contains((string)aValue);
            }
            return false;
        }
    }
}
