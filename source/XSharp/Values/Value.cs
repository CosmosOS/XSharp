using System;
using System.Collections.Generic;
using System.Text;

namespace XSharp.Values {
  public abstract class Value {
    public readonly string RawText;

    public Value(string aText) {
      RawText = aText;
    }
  }
}
