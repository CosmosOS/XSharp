using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace XSharp.Tokens {
  public class Root : Token {
    public Root() {
      // Load emitters to pattern list
      foreach (var xMethod in GetType().GetRuntimeMethods()) {
        var xAttrib = xMethod.GetCustomAttribute<EmitterAttribute>();
        if (xAttrib != null) {
          AddPattern(
            (Compiler aCompiler, List<CodePoint> aPoints) => {
              var xArgs = new List<object>();
              xArgs.Add(aCompiler);
              xArgs.AddRange(aPoints.Select(q => q.Value));
              xMethod.Invoke(this, xArgs.ToArray());
            }
            , xAttrib.TokenTypes);
        }
      }
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

    [Emitter(typeof(Register), typeof(Assignment), typeof(Number64u))]
    protected void Emit_RegAssignNum(Compiler aCompiler, string aReg, string aEquals, UInt64 aVal) {
      aCompiler.WriteLine($"mov {aReg}, 0x{aVal:X}");
    }
  }
}
