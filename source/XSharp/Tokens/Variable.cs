namespace XSharp.Tokens
{
    public class Variable : Spruce.Tokens.Token
    {
        public Variable() : base(Chars.AlphaNum, ".")
        {
        }

        protected override object Check(string aText)
        {
            return aText.Substring(1);
        }
    }

    public class VariableAddress : Spruce.Tokens.Token
    {
        public VariableAddress() : base(Chars.AlphaNum, "@")
        {
        }

        protected override bool CheckChar(int aLocalPos, char aChar)
        {
            switch (aLocalPos)
            {
                case 0:
                    return aChar == '@';

                case 1:
                    return aChar == '.';
            }
            return base.CheckChar(aLocalPos, aChar);
        }

        protected override object Check(string aText)
        {
            return aText.Substring(2);
        }
    }
}
