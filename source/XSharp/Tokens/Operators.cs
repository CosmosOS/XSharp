using Spruce.Tokens;

namespace XSharp.Tokens
{
    public class OpComment : MatchOne
    {
        // Comments require a space after. Prevents future conflicts with 3 char ones like Literal
        public OpComment() : base(@"// ")
        {
        }
    }

    public class OpLiteral : MatchOne
    {
        public OpLiteral() : base(@"//!")
        {
        }
    }

    public class OpComparision : MatchList
    {
        public OpComparision() : base("< > = <= >= !=".Split(' '))
        {
        }
    }

    public class OpArithmetic : MatchList
    {
        public OpArithmetic() : base("+ - * / %".Split(' '))
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

    public class OpIncrementDecrement : MatchList
    {
        public OpIncrementDecrement() : base("++ --".Split(' '))
        {
        }
    }

    public class OpBitwiseShift : MatchList
    {
        public OpBitwiseShift() : base("<< >>".Split(' '))
        {
        }
    }
}
