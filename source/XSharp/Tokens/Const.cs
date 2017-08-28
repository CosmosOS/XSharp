namespace XSharp.Tokens
{
    public class Const : Spruce.Tokens.Token
    {
        public Const() : base(Chars.AlphaNum, "#")
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
