using System;
using System.Collections.Generic;
using System.Text;

namespace XSharp.Tokens {
    public class Identifier : Spruce.Tokens.AlphaNum {
        public Identifier() : base("_", "_") {
        }
    }
}
