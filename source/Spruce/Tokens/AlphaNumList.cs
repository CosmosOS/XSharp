namespace Spruce.Tokens
{
    public class AlphaNumList : StringList {
        protected AlphaNumList(string aText, bool aIgnoreCase = true) : this(new[] { aText }, aIgnoreCase) { }
        protected AlphaNumList(string[] aList, bool aIgnoreCase = true) : base(aList, Chars.AlphaNum, aIgnoreCase) { }
    }
}
