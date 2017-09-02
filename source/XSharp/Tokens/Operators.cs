using System.Runtime.CompilerServices;
using Spruce.Tokens;

namespace XSharp.Tokens
{
    public class OpComment : StringList
    {
        // Comments require a space after. Prevents future conflicts with 3 char ones like Literal
        public OpComment() : base(@"//")
        {
        }
    }

    public class OpLiteral : StringList
    {
        public OpLiteral() : base(@"//!")
        {
        }
    }

    public class OpCompare : StringList
    {
        public OpCompare() : base("< > = <= >= !=".Split(' '))
        {
        }
    }

    public class OpMath : StringList
    {
        public OpMath() : base("+ - * / %".Split(' '))
        {
        }
    }

    // Only used for bitwise ops with two parameters
    public class OpBitwise : StringList
    {
        public OpBitwise() : base("& | ^".Split(' '))
        {
        }
    }

    public class OpIncDec : StringList
    {
        public OpIncDec() : base("++ --".Split(' '))
        {
        }
    }

    public class OpShift : StringList
    {
        public OpShift() : base("<< >>".Split(' '))
        {
        }
    }

    public class OpRotate : StringList
    {
        public OpRotate() : base("~> <~".Split(' '))
        {
        }
    }

    public class OpPureComparators : StringList
    {
        public OpPureComparators() : base("=0 !0 > < >= <= = !".Split(' '))
        {
        }
    }
}
