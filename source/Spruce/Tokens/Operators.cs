namespace Spruce.Tokens {
    public class OpEquals : StringList {
        public OpEquals() : base("=") {
        }
    }

    public class OpSlashBack : StringList {
        public OpSlashBack() : base(@"\") {
        }
    }

    public class OpSlashFwd : StringList {
        public OpSlashFwd() : base("/") {
        }
    }

    public class OpPlus : StringList {
        public OpPlus() : base("+") {
        }
    }

    public class OpMinus : StringList {
        public OpMinus() : base("-") {
        }
    }

    public class OpStar : StringList {
        public OpStar() : base("*") {
        }
    }

    public class OpPipe : StringList {
        public OpPipe() : base("|") {
        }
    }

    public class OpTilde : StringList {
        public OpTilde() : base("~") {
        }
    }

    public class OpPercent : StringList {
        public OpPercent() : base("%") {
        }
    }

    public class OpCaret : StringList {
        public OpCaret() : base("^") {
        }
    }

    public class OpAt : StringList {
        public OpAt() : base("@") {
        }
    }

    public class OpAnd : StringList {
        public OpAnd() : base("&") {
        }
    }

    public class OpOpenBrace : StringList {
        public OpOpenBrace() : base("{") {
        }
    }

    public class OpCloseBrace : StringList {
        public OpCloseBrace() : base("}") {
        }
    }

    public class OpOpenParen : StringList {
        public OpOpenParen() : base("(") {
        }
    }

    public class OpCloseParen : StringList {
        public OpCloseParen() : base(")") {
        }
    }

    public class OpOpenBracket : StringList {
        public OpOpenBracket() : base("[") {
        }
    }

    public class OpCloseBracket : StringList {
        public OpCloseBracket() : base("]") {
        }
    }

    public class OpColon : StringList {
        public OpColon() : base(":") {
        }
    }
}
