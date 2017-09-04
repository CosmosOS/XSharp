using System.Linq;

namespace XSharp.x86.Params
{
    public class MemoryAddress : Identifier
    {
        public override bool IsMatch(object aValue)
        {
            return aValue is string value && value.First() == '[' && value.Last() == ']' && base.IsMatch(value.Substring(1, value.Length - 2));
        }
    }
}
