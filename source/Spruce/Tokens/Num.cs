using System;
using System.Collections.Generic;
using System.Text;
using Parsers = Spruce.Parsers;

namespace Spruce.Tokens {
    public abstract class Num<T> : TypedToken<T> {
        protected Num(Parsers.Parser aParser) : base(aParser) { }
    }
}
