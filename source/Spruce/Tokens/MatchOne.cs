using System;
using System.Collections.Generic;
using System.Text;

namespace Spruce.Tokens {
    // Currently just inherits from MatchList, but separate
    // to allow future movement out of it or optimizations.
    public class MatchOne : MatchList {
        protected MatchOne(string aText) : base(new[] { aText }) { }
    }
}
