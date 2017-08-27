namespace Spruce.Tokens
{
    public class OpEquals : MatchOne
    {
        public OpEquals() : base("=")
        {
        }
    }

    public class OpSlashBack : MatchOne
    {
        public OpSlashBack() : base(@"\")
        {
        }
    }

    public class OpSlashFwd : MatchOne
    {
        public OpSlashFwd() : base("/")
        {
        }
    }

    public class OpPlus : MatchOne
    {
        public OpPlus() : base("+")
        {
        }
    }

    public class OpMinus : MatchOne
    {
        public OpMinus() : base("-")
        {
        }
    }

    public class OpStar : MatchOne
    {
        public OpStar() : base("*")
        {
        }
    }

    public class OpPipe : MatchOne
    {
        public OpPipe() : base("|")
        {
        }
    }

    public class OpTilde : MatchOne
    {
        public OpTilde() : base("~")
        {
        }
    }

    public class OpPercent : MatchOne
    {
        public OpPercent() : base("%")
        {
        }
    }

    public class OpCaret : MatchOne
    {
        public OpCaret() : base("^")
        {
        }
    }

    public class OpAt : MatchOne
    {
        public OpAt() : base("@")
        {
        }
    }

    public class OpAnd : MatchOne
    {
        public OpAnd() : base("&")
        {
        }
    }

    public class OpOpenBrace : MatchOne
    {
        public OpOpenBrace() : base("{")
        {
        }
    }

    public class OpCloseBrace : MatchOne
    {
        public OpCloseBrace() : base("}")
        {
        }
    }

    public class OpOpenParen : MatchOne
    {
        public OpOpenParen() : base("(")
        {
        }
    }

    public class OpCloseParen : MatchOne
    {
        public OpCloseParen() : base(")")
        {
        }
    }

    public class OpColon : MatchOne
    {
        public OpColon() : base(":")
        {
        }
    }
}
