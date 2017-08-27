namespace Spruce.Tokens
{
    public class AlphaNum : Token
    {
        public AlphaNum() : this("") { }
        // First char cannot be digit
        public AlphaNum(string aExtraChars, string aExtraFirstChars = "") : base(aExtraChars + Chars.AlphaNum, aExtraFirstChars + Chars.AlphaNum) { }

        public override object Check(string aText)
        {
            return aText;
        }
    }
}
