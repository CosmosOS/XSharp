namespace XSharp.Tokens
{
    public class NamespaceKeyword : Spruce.Tokens.AlphaNumList
    {
        public NamespaceKeyword() : base("Namespace")
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

    public class GotoKeyword : Spruce.Tokens.AlphaNumList
    {
        public GotoKeyword() : base("goto")
        {
        }
    }

    public class TestKeyword : Spruce.Tokens.AlphaNumList
    {
        public TestKeyword() : base("test")
        {
        }
    }

    public class PortKeyword : Spruce.Tokens.AlphaNumList
    {
        public PortKeyword() : base("port")
        {
        }
    }
}
