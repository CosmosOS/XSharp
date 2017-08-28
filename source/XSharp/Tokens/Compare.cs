using System;
using System.Collections.Generic;
using System.Text;
using Spruce.Attribs;

namespace XSharp.Tokens {
    [GroupToken(typeof(Compare08), typeof(Compare16), typeof(Compare32))]
    public class Compare : Spruce.Tokens.Compound {
    }

    public class Compare08 : Compare {
    }

    public class Compare16 : Compare {
    }

    public class Compare32 : Compare {
        public Compare32() {
            mInternals.Add(new Reg32());
            mInternals.Add(new OpCompare());
            mInternals.Add(new Int32u());
        }
    }
}
