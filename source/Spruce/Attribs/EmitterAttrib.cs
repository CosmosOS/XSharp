using System;
using System.Collections.Generic;
using System.Text;

namespace Spruce.Attribs {
  [AttributeUsage(System.AttributeTargets.Method)]
  public class Emitter : Attribute {
    public readonly Type[] TokenTypes;

    public Emitter(params Type[] aParams) {
      TokenTypes = aParams;
    }
  }
}
