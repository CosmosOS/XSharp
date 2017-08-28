namespace XSharp.Tokens
{
    public class Variable : Spruce.Tokens.AlphaNum
    {
        public Variable() : base(Chars.AlphaNum, ".")
        {
        }

        protected override bool CheckChar(int aLocalPos, char aChar)
        {
            // The name of the variable must start with a alphabet
            if (aLocalPos == 1)
            {
                return Chars.Alpha.IndexOf(aChar) > -1;
            }
            return base.CheckChar(aLocalPos, aChar);
        }

        public override object Check(string aText)
        {
            return aText.Substring(1);
        }
    }

    public class VariableAddress : Spruce.Tokens.AlphaNum
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

                case 2:
                    return Chars.Alpha.IndexOf(aChar) > -1;
            }
            return base.CheckChar(aLocalPos, aChar);
        }

        public override object Check(string aText)
        {
            return aText.Substring(2);
        }
    }
}
