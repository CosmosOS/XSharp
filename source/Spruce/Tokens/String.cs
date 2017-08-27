namespace Spruce.Tokens
{
    public class String : Token
    {
        public override object Check(string aText)
        {
            return null;
        }

        // We are overriding the parse method to set rStart itself
        public override object Parse(string aText, ref int rStart)
        {
            int i;

            var isEscaped = false;
            var foundEndQuote = false;

            for (i = 1; i < aText.Length - rStart; i++)
            {
                if (aText[rStart + i] == '\\')
                {
                    isEscaped = true;
                    continue;
                }

                if (aText[rStart + i] == '\'' && !isEscaped)
                {
                    foundEndQuote = true;
                    i += 1; // Increment i so that the end quote is also included
                    break;
                }

                isEscaped = false;
            }

            // Cannot find end quote. We cannot parse the object as string.
            if (!foundEndQuote)
                return null;

            // Replace all \' with ' and then remove the opening quotes
            var xResult = aText.Substring(rStart + 1, i - 2).Replace(@"\'", @"'");
            rStart += i;
            return xResult;
        }
    }
}
