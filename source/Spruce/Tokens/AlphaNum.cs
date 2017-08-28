namespace Spruce.Tokens
{
    public class AlphaNum : Token
    {
        public AlphaNum() : this(Chars.AlphaNum)
        {
        }

        // First char cannot be digit
        public AlphaNum(string aExtraChars, string aExtraFirstChars = "") : base(aExtraChars + Chars.AlphaNum, aExtraFirstChars + Chars.Alpha)
        {
        }

        public override object Check(string aText)
        {
            return aText;
        }
    }
}
