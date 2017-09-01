namespace XSharp.Tokens
{
    public class Namespace : Spruce.Tokens.AlphaNumList
    {
        public Namespace() : base("Namespace")
        {
        }
    }

    public class Function : Spruce.Tokens.AlphaNumList
    {
        public Function() : base("Function")
        {
        }
    }

    public class Interrupt : Spruce.Tokens.AlphaNumList
    {
        public Interrupt() : base("Interrupt")
        {
        }
    }

    public class If : Spruce.Tokens.AlphaNumList
    {
        public If() : base("If")
        {
        }
    }

    public class While : Spruce.Tokens.AlphaNumList
    {
        public While() : base("While")
        {
        }
    }

    public class Repeat : Spruce.Tokens.AlphaNumList
    {
        public Repeat() : base("Repeat")
        {
        }
    }

    public class Times : Spruce.Tokens.AlphaNumList
    {
        public Times() : base("Times")
        {
        }
    }

    public class ConstKeyword : Spruce.Tokens.AlphaNumList
    {
        public ConstKeyword() : base("Const")
        {
        }
    }

    public class VarKeyword : Spruce.Tokens.AlphaNumList
    {
        public VarKeyword() : base("Var")
        {
        }
    }

    public class AsKeyword : Spruce.Tokens.AlphaNumList
    {
        public AsKeyword() : base("as")
        {
        }
    }
}
