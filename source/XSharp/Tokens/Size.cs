using System;
using System.Collections.Generic;
using System.Text;

namespace XSharp.Tokens
{
    public class Size : Spruce.Tokens.StringList
    {
        public Size() : base("byte word dword".Split(' '))
        { }
    }
}
