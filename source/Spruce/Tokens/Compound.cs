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
            int xThisStart = rStart;
            var xResult = new List<object>();
            foreach (var xToken in mInternals) {
                SkipWhiteSpace(aText, ref xThisStart);
                var xParseResult = (xToken.Parse(aText, ref xThisStart));
                if (xParseResult == null) {
                    return null;
                }
                xResult.Add(xParseResult);
            }
            rStart = xThisStart;
            return xResult.ToArray();
        }

    }
}
