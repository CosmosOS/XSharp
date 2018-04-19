namespace XSharp.Tokens
{
    public class Size : Spruce.Tokens.StringList
    {
        public Size() : base("byte word dword".Split(' '))
        { }
    }
}
