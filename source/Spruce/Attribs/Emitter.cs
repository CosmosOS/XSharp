using System;
using System.Collections.Generic;
using System.Text;

namespace Spruce.Attribs {
  [AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
  public class Emitter : Attribute {
    public readonly Type[] TokenTypes;

    public Emitter(params Type[] aParams) {
      TokenTypes = aParams;
    }
  }
}
