using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Spruce.Tokens {
    public abstract class IdList : TypedToken<string> {
        protected string[] mList;

        protected IdList(string[] aList, bool aUpperResult = true) : base(aUpperResult ? Parsers.Parsers.IdentifierUpper : Parsers.Parsers.Identifier) {
            mList = aList;
        }

        protected override bool IsMatch(ref string rValue) {
            return mList.Contains(rValue);
        }
    }
}
