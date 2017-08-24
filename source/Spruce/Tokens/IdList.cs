using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Spruce.Tokens {
    public abstract class IdList : ID {
        protected string[] mList;

        protected IdList(string[] aList, bool aUpper = true) : base(null, aUpper) {
            mList = aList;
        }

        protected override bool IsMatch(ref string rValue) {
            return mList.Contains(rValue);
        }
    }
}
