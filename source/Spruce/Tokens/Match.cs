using System;
using System.Collections.Generic;
using System.Text;

namespace Spruce.Tokens {
    // Currently just inherits from MatchList, but separate
    // to allow future movement out of it or optimizations.
    public class Match : MatchList {
        protected Match(string aText) : base(new[] { aText }) { }
    }
}
