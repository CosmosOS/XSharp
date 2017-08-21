using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XSharp.Tokens {
  public class Root : Token {
    public Root() {
      AddPattern(Emit__Reg_Assign_Num, typeof(Register), typeof(Assignment), typeof(Number64u));
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

    protected void Emit__Reg_Assign_Num(Compiler aCompiler, List<CodePoint> aPoints) {
      var xReg = (string)aPoints[0].Value;
      var xVal = ((UInt64)aPoints[2].Value).ToString("X");
      aCompiler.WriteLine($"mov {xReg}, 0x{xVal}");
    }
  }
}
