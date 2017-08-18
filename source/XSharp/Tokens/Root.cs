using System;
using System.Collections.Generic;
using System.Text;

namespace XSharp.Tokens {
  public class Root : Token {
    public Root() {
      Tokens.Add(new Register());
    }

    protected override bool IsMatch(object aValue) {
      // Dont foresee this ever being called in Root, but....
      return true;
    }

    public List<CodePoint> Parse(string aText) {
      var xResult = new List<CodePoint>();
      int aPos = 0;
      var xCP = Next(aText, ref aPos);
      xResult.Add(xCP);
      return xResult;
    }
  }
}
