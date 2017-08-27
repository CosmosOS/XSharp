using System;
using System.Collections.Generic;
using System.Text;

namespace Spruce.Tokens {
    // Could use attribs to build descendants up like Groups,
    // but compounds will usually add functionality anyway and
    // may use instanced Tokens rather than default ctors so
    // it is done without attributes.
    public class Compound : Token {
        // Tokens that make up the compound
        protected List<Token> mInternals = new List<Token>();

        public override object Parse(string aText, ref int rStart) {
            int xOrigStart = rStart;
            foreach (var xToken in mInternals) {
                if (xToken.Parse(aText, ref rStart) == null) {
                    rStart = xOrigStart;
                    return null;
                }
            }
            return null;
        }

    }
}
