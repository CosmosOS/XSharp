using System;
using System.Collections.Generic;
using System.Text;
using Spruce.Tokens;

namespace XSharp.Tokens {
  public class OpComment : Match {
    // Comments require a space after. Prevents future conflicts with 3 char ones like Literal
    public OpComment() : base(@"// ") { }
  }

  public class OpLiteral : Match {
    public OpLiteral() : base(@"//!") { }
  }

}
