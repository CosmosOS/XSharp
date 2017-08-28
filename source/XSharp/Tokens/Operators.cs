using Spruce.Tokens;

namespace XSharp.Tokens
{
    public class OpComment : MatchList
    {
        // Comments require a space after. Prevents future conflicts with 3 char ones like Literal
        public OpComment() : base(@"// ")
        {
        }
    }

    public class OpLiteral : MatchList {
        public OpLiteral() : base(@"//! ")
        {
        }
    }

    public class OpCompare : MatchList
    {
        public OpCompare() : base("< > = <= >= !=".Split(' '))
        {
        }
    }

    public class OpMath : MatchList
    {
        public OpMath() : base("+ - * / %".Split(' '))
        {
        }
    }

    // Only used for bitwise ops with two parameters
    public class OpBitwise : MatchList
    {
        public OpBitwise() : base("& | ^".Split(' '))
        {
        }
    }

    public class OpIncDec : MatchList
    {
        public OpIncDec() : base("++ --".Split(' '))
        {
        }
    }

    public class OpShift : MatchList
    {
        public OpShift() : base("<< >>".Split(' '))
        {
        }
    }
}
