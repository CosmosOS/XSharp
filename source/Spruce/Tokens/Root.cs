using System;
using System.Collections.Generic;
using System.Text;

namespace Spruce.Tokens {
    public abstract class Root : Token {
        public Root(Parsers.Parser aParser) : base(aParser) { }
    }
}
