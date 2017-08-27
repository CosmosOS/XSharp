namespace Spruce.Tokens
{
    public class AlphaNum : Token
    {
        // First char cannot be digit
        public AlphaNum() : base(Chars.AlphaNum, Chars.Alpha) { }

        public override object Check(string aText)
        {
            return aText;
        }
    }
}
