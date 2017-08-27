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
            var xResult = new List<object>();
            foreach (var xToken in mInternals) {
                var xParseResult = (xToken.Parse(aText, ref rStart));
                if (xParseResult == null) {
                    // Not all internal tokens match, restore rStart and return null.
                    rStart = xOrigStart;
                    return null;
                }
                xResult.Add(xParseResult);
            }
            return xResult;
        }

    }
}
