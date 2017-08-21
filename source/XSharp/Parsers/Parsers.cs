using System;
using System.Collections.Generic;
using System.Text;

namespace XSharp.Parsers {
  public static class Parsers {
    public static readonly Identifier Identifier = new Identifier();
    public static readonly Number64u Number64u = new Number64u();
    public static readonly Operator Operator = new Operator();
  }
}
