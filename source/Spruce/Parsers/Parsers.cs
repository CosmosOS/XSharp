using System;
using System.Collections.Generic;
using System.Text;

namespace Spruce.Parsers {
  public static class Parsers {
    public static readonly All All = new All();
    public static readonly Identifier Identifier = new Identifier();
    public static readonly Identifier IdentifierUpper = new Identifier(true);
    public static readonly Number64u Number64u = new Number64u();
    public static readonly Operator Operator = new Operator();
  }
}
