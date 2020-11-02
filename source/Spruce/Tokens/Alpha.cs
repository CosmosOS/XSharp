using System;

namespace Spruce.Tokens
{
    public class Alpha : Token
    {
        public Alpha() : base(Chars.Alpha) { }

        protected override bool Check(string aText)
        {
            if (aText.Length != 1)
            {
                throw new ArgumentOutOfRangeException("Alpha should only be a single character.");
            }

            return true;
        }
    }
}
