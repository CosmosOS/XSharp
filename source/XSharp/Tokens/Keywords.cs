namespace XSharp.Tokens
{
    public class Namespace : Spruce.Tokens.AlphaNumList
    {
        public Namespace() : base("Namespace")
        {
        }
    }

    public class Function : Spruce.Tokens.AlphaNumList {
        public Function() : base("Function")
        {
        }
    }

    public class If : Spruce.Tokens.AlphaNumList {
        public If() : base("If")
        {
        }
    }

    public class ConstKeyword : Spruce.Tokens.AlphaNumList {
        public ConstKeyword() : base("Const")
        {
        }
    }

    public class VarKeyword : Spruce.Tokens.AlphaNumList {
        public VarKeyword() : base("Var")
        {
        }
    }

    public class AsKeyword : Spruce.Tokens.AlphaNumList {
        public AsKeyword() : base("as")
        {
        }
    }
}
