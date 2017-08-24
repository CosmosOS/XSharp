using System;
using System.Collections.Generic;
using System.Text;

namespace Spruce.Parsers {
    public static class Parsers {
        public static readonly All All = new All();
        public static readonly Identifier Identifier = new Identifier();
        public static readonly Identifier IdentifierUpper = new Identifier(true);
        public static readonly Num08u Num08u = new Num08u();
        public static readonly Num16u Num16u = new Num16u();
        public static readonly Num32u Num32u = new Num32u();
        public static readonly Operator Operator = new Operator();
    }
}
