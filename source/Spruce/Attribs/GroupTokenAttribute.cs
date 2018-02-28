using System;

namespace Spruce.Attribs {
    // Could search descendants of a base like RegXX to the various registers,
    // but it would be less reliable and less flexible.
    // The attribute for example can belong to any class, even dummy one.
    [AttributeUsage(AttributeTargets.Class)]
    public class GroupTokenAttribute : Attribute {
        public Type[] TokenTypes { get; }

        public GroupTokenAttribute(params Type[] tokenTypes) {
            TokenTypes = tokenTypes;
        }
    }
}
