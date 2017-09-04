using System.Linq;

namespace XSharp.x86.Params
{
    public class Identifier : Param
    {
        public override bool IsMatch(object aValue)
        {
            if (!(aValue is string value)) return false;
            return (value.All(c => char.IsLetter(c) || char.IsNumber(c) || c == '_') && !char.IsDigit(value[0]));
        }
    }
}
