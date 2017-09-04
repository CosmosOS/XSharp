using System.Linq;

namespace XSharp.x86.Params
{
    public class SingleWord : Param
    {
        public override bool IsMatch(object aValue)
        {
            return aValue is string value && !value.Contains(' ');
        }
    }
}
