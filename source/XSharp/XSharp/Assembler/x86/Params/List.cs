using System;
using System.Collections.Generic;
using System.Linq;

namespace XSharp.x86.Params
{
    public class List : Param {
        public List(IReadOnlyList<string> aTexts) {
            Texts = aTexts;
        }

        protected IReadOnlyList<string> Texts { get; }

        public override object Transform(object aValue) {
            return ((string)aValue).ToUpper();
        }

        public override bool IsMatch(object aValue) {
            if (aValue is string) {
                return Texts.Contains((string)aValue, StringComparer.CurrentCultureIgnoreCase);
            }
            return false;
        }

    }
}
