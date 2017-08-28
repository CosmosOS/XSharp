namespace XSharp.Tokens
{
    public class Constant : Spruce.Tokens.Token
    {
        public Constant() : base(Chars.AlphaNum, "#")
        {
        }

        protected override bool CheckChar(int aLocalPos, char aChar)
        {
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
}
