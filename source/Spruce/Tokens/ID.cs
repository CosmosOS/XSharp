using System;
using System.Collections.Generic;
using System.Text;

namespace Spruce.Tokens {
    public class ID : TypedToken<string> {
        protected string mText;

        public ID() : base(Parsers.Parsers.Identifier) {
        }
        public ID(string aText, bool aUpperResult = true) : base(aUpperResult ? Parsers.Parsers.IdentifierUpper : Parsers.Parsers.Identifier) {
            mText = aText;
        }

        protected override bool IsMatch(ref string rValue) {
            return rValue == mText;
        }
    }
}
