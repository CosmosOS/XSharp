namespace Spruce.Tokens
{
    public class AlphaNum : Token
    {
        public AlphaNum() : this("")
        {
        }

        // First char cannot be digit
        public AlphaNum(string aExtraChars) : base(aExtraChars + Chars.AlphaNum, Chars.Alpha)
        {
        }

        public AlphaNum(string aExtraChars, string aFirstChars = "") : base(aExtraChars + Chars.AlphaNum, aFirstChars)
        {
        }

        public override object Check(string aText)
        {
            return aText;
        }
    }
}
