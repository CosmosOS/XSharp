using System;
using System.Collections.Generic;
using System.Text;

namespace XSharp.Tokens {
  public class Namespace : Spruce.Tokens.MatchList {
    public Namespace() : base("Namespace") { }
  }

  public class NOP : Spruce.Tokens.MatchList {
    public NOP() : base("NOP") { }
  }

  public class Return : Spruce.Tokens.MatchList {
    public Return() : base("Return") { }
  }
}
