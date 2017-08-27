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

    public class Const : Spruce.Tokens.MatchOne
    {
        public Const() : base("Const")
        {
        }
    }

    public class Var : Spruce.Tokens.MatchOne
    {
        public Var() : base("Var")
        {
        }
    }
}
