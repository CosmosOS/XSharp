using System.Linq;
using Spruce.Attribs;
using Spruce.Tokens;
using XSharp.x86;
using XSharp.x86.Params;

namespace XSharp.Tokens
{
    [GroupToken(typeof(RegAddr08), typeof(RegAddr16), typeof(RegAddr32))]
    public class RegAddr : Reg
    {
        protected RegAddr(string aText) : this(new[] { $"[{aText}]" })
        {
        }

        protected RegAddr(string[] aList) : base(aList.Select(s => $"[{s}]").ToArray())
        {
        }

        protected override object Transform(string aText)
        {
            return new Address(new Register(aText.Substring(1, aText.Length - 2)));
        }
    }

    public class RegAddr08 : RegAddr
    {
        public RegAddr08() : base(x86.Register.Names.Reg08)
        {
        }
    }

    public class RegAddr16 : RegAddr
    {
        public RegAddr16() : base(x86.Register.Names.Reg16)
        {
        }
    }

    public class RegAddr32 : RegAddr
    {
        public RegAddr32() : base(x86.Register.Names.Reg32)
        {
        }
    }

    public class MemAddr : AlphaNum
    {
        public MemAddr() : base("_")
        {
            mFirstChars = "[";
        }

        protected override bool Check(string aText)
        {
            return aText.Last() == ']' && base.Check(aText);
        }

        protected override object Transform(string aText)
        {
            return new Address(aText.Substring(1, aText.Length - 2));
        }
    }
}
