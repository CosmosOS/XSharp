using System;
using System.Collections.Generic;
using System.Text;

namespace Spruce.Tokens
{
    public class OpEquals : MatchOne {
        public OpEquals() : base("=") { }
    }

    public class OpSlashBack : MatchOne {
        public OpSlashBack() : base(@"\") { }
    }

    public class OpSlashFwd : MatchOne {
        public OpSlashFwd() : base("/") { }
    }

    public class OpPlus : MatchOne {
        public OpPlus() : base("+") { }
    }

    public class OpMinus : MatchOne {
        public OpMinus() : base("-") { }
    }

    public class OpStar : MatchOne {
        public OpStar() : base("*") { }
    }

    public class OpPipe : MatchOne {
        public OpPipe() : base("|") { }
    }

    public class OpTilde : MatchOne {
        public OpTilde() : base("~") { }
    }

    public class OpPercent : MatchOne {
        public OpPercent() : base("%") { }
    }

    public class OpCaret : MatchOne {
        public OpCaret() : base("^") { }
    }

    public class OpAt : MatchOne {
        public OpAt() : base("@") { }
    }
}
