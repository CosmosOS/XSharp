using System;
using System.Collections.Generic;
using System.Text;

namespace XSharp {
  [AttributeUsage(System.AttributeTargets.Method)]
  public class EmitterAttribute : Attribute {
    public readonly Type[] TokenTypes;

    public EmitterAttribute(params Type[] aParams) {
      TokenTypes = aParams;
    }
  }
}
