namespace Spruce.Tokens
{
    public class StringList : ObjectList<string> {
        protected StringList(string aText, string aNoobChars = "", bool aIgnoreCase = true) : this(new[] { aText }, aNoobChars, aIgnoreCase) { }
        protected StringList(string[] aList, string aNoobChars = "", bool aIgnoreCase = true) : base(aList, aList, aNoobChars, aIgnoreCase) { }
    }
}
