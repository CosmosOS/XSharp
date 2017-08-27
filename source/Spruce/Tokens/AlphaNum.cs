namespace Spruce.Tokens
{
    public class AlphaNum : Token
    {
        // First char cannot be digit
        public AlphaNum() : base(Chars.AlphaNum, Chars.Alpha) { }

        protected override object Check(string aText)
        {
            return aText;
        }
    }
}
