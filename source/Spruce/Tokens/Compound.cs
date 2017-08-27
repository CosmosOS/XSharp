using System;
using System.Collections.Generic;
using System.Text;

namespace Spruce.Tokens {
    public class Compound : Token {
        // Tokens that make up the compound
        protected List<Token> mInternals = new List<Token>();

        protected override object Check(string aText) {
            throw new NotImplementedException();
        }
    }
}
