namespace Spruce.Tokens {
    public class OpEquals : MatchList {
        public OpEquals() : base("=") {
        }
    }

    public class OpSlashBack : MatchList {
        public OpSlashBack() : base(@"\") {
        }
    }

    public class OpSlashFwd : MatchList {
        public OpSlashFwd() : base("/") {
        }
    }

    public class OpPlus : MatchList {
        public OpPlus() : base("+") {
        }
    }

    public class OpMinus : MatchList {
        public OpMinus() : base("-") {
        }
    }

    public class OpStar : MatchList {
        public OpStar() : base("*") {
        }
    }

    public class OpPipe : MatchList {
        public OpPipe() : base("|") {
        }
    }

    public class OpTilde : MatchList {
        public OpTilde() : base("~") {
        }
    }

    public class OpPercent : MatchList {
        public OpPercent() : base("%") {
        }
    }

    public class OpCaret : MatchList {
        public OpCaret() : base("^") {
        }
    }

    public class OpAt : MatchList {
        public OpAt() : base("@") {
        }
    }

    public class OpAnd : MatchList {
        public OpAnd() : base("&") {
        }
    }

    public class OpOpenBrace : MatchList {
        public OpOpenBrace() : base("{") {
        }
    }

    public class OpCloseBrace : MatchList {
        public OpCloseBrace() : base("}") {
        }
    }

    public class OpOpenParen : MatchList {
        public OpOpenParen() : base("(") {
        }
    }

    public class OpCloseParen : MatchList {
        public OpCloseParen() : base(")") {
        }
    }

    public class OpOpenBracket : MatchList {
        public OpOpenBracket() : base("[") {
        }
    }

    public class OpCloseBracket : MatchList {
        public OpCloseBracket() : base("]") {
        }
    }

    public class OpColon : MatchList {
        public OpColon() : base(":") {
        }
    }
}
