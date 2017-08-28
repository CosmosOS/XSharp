namespace XSharp.Tokens
{
    public class Variable : Identifier
    {
        public Variable() {
            mFirstChars = ".";
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

    public class VariableAddress : Identifier
    {
        public VariableAddress()
        {
            mFirstChars = "@";
        }

        protected override bool CheckChar(int aLocalPos, char aChar)
        {
            switch (aLocalPos)
            {
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
