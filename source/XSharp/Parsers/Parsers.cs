using System;
using System.Collections.Generic;
using System.Text;

namespace XSharp.Parsers {
  public static class Parsers {
    public static readonly Identifier Identifiers = new Identifier();
    public static readonly Number Numbers = new Number();
    public static readonly Operator Operators = new Operator();
  }
}
