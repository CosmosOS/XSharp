using System;
using System.Collections.Generic;
using System.Text;
using Spruce.Tokens;

namespace XSharp.Tokens {
  public class OpComment : Op {
    public OpComment() : base(@"//") { }
  }

  public class OpLiteral : Op {
    public OpLiteral() : base(@"//!") { }
  }

}
