namespace XSharp.Tokens
{
    public class Const : Identifier
    {
        public Const()
        {
            mFirstChars = "#";
        }

        protected override bool CheckChar(int aLocalPos, char aChar)
        {
            if (aLocalPos == 1)
            {
                return Chars.Alpha.IndexOf(aChar) > -1;
            }
            return base.CheckChar(aLocalPos, aChar);
        }

        protected override object Transform(string aText)
        {
            return aText.Substring(1);
        }
    }
}
