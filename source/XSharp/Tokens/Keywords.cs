namespace XSharp.Tokens
{
    public class Namespace : Spruce.Tokens.MatchOne
    {
        public Namespace() : base("Namespace")
        {
        }
    }

    public class Function : Spruce.Tokens.MatchOne
    {
        public Function() : base("Function")
        {
        }
    }

    public class If : Spruce.Tokens.MatchOne
    {
        public If() : base("If")
        {
        }
    }

    public class ConstKeyword : Spruce.Tokens.MatchOne
    {
        public ConstKeyword() : base("Const")
        {
        }
    }

    public class VarKeyword : Spruce.Tokens.MatchOne
    {
        public VarKeyword() : base("Var")
        {
        }
    }
}
