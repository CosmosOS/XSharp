using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XSharp.Tokens {
  public class RegXX : Reg {
    public static readonly string[] Names;

    static RegXX() {
      var xNames = new List<string>();
      xNames.AddRange(Reg32.Names);
      xNames.AddRange(Reg16.Names);
      xNames.AddRange(Reg08.Names);
      Names = xNames.ToArray();
    }

    protected override string[] GetList() {
      return Names;
    }
  }
}
