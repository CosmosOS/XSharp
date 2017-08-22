using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XSharp.Tokens {
  public class RegisterXX : Register {
    public static readonly string[] Names;

    static RegisterXX() {
      var xNames = new List<string>();
      xNames.AddRange(Register32.Names);
      xNames.AddRange(Register16.Names);
      xNames.AddRange(Register08.Names);
      Names = xNames.ToArray();
    }

    protected override string[] GetList() {
      return Names;
    }
  }
}
