using System;

namespace Spruce.Attribs {
  [AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
  public sealed class EmitterAttribute : Attribute {
    public Type[] TokenTypes { get; }

    public EmitterAttribute(params Type[] tokenTypes) {
      TokenTypes = tokenTypes;
    }
  }
}
