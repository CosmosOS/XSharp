using System;
using System.Collections.Generic;
using System.Text;

namespace Spruce.Attribs {
    // Could search descendants of a base like RegXX to the various registers,
    // but it would be less reliable and less flexible.
    // The attribute for example can belong to any class, even dummy one.
    [AttributeUsage(AttributeTargets.Class)]
    public class GroupToken : Attribute {
        public readonly Type[] TokenTypes;

        public GroupToken(params Type[] aParams) {
            TokenTypes = aParams;
        }
    }
}
