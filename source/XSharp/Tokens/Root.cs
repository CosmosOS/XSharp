using System;
using System.Collections.Generic;
using System.Text;

namespace XSharp.Tokens {
  public class Root : Token {
    public Root() {
      Tokens.Add(new Register());
    }

    protected override bool IsMatch(string aText) {
      // Dont foresee this ever being called in Root, but....
      return true;
    }

    public List<Values.Value> Parse(string aText) {
      var xResult = new List<Values.Value>();
      int aPos = 0;
      var xValue = Next(aText, ref aPos);
      return xResult;
    }
  }
}
