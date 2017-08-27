namespace XSharp.Tokens
{
    public class Constant : Spruce.Tokens.Token
    {
        public Constant() : base(Chars.AlphaNum, "#")
        {
        }

        public override object Check(string aText)
        {
            return aText.Substring(1);
        }
    }
}
