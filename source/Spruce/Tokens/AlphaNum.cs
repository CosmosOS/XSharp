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

        // CAREFUL. Matches ALL, but is proper behaviour and needed.
        protected override bool Check(string aText) {
            return true;
        }
    }
}
