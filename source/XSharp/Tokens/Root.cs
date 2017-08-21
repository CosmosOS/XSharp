using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XSharp.Tokens {
  public class Root : Token {
    public Root() {
      AddPattern(Emit_RegisterAssign, typeof(Register), typeof(Assignment), typeof(Number64u));
    }

    protected override bool IsMatch(object aValue) {
      // Dont foresee this ever being called in Root, but....
      return true;
    }

    public List<CodePoint> Parse(string aText) {
      // Important for end detection. Do not TrimStart, will goof up CodePoint indexes.
      aText = aText.TrimEnd();
      var xResult = new List<CodePoint>();
      int aPos = 0;
      Token xToken = this;

      while (aPos < aText.Length) {
        var xCP = xToken.Next(aText, ref aPos);
        if (xCP == null) {
          break;
        }
        xToken = xCP.Token;
        xResult.Add(xCP);
      }

      return xResult;
    }

    protected void Emit_RegisterAssign(Compiler aCompiler, List<CodePoint> aCodePoints) {
      aCompiler.WriteLine("TEST");
    }
  }
}
