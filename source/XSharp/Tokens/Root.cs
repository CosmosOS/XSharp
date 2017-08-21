using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace XSharp.Tokens {
  public class Root : Token {
    public Root() {
      // Load emitters to pattern list
      foreach (var xMethod in typeof(Emitters).GetRuntimeMethods()) {
        var xAttrib = xMethod.GetCustomAttribute<EmitterAttribute>();
        if (xAttrib != null) {
          AddPattern(
            (Compiler aCompiler, List<CodePoint> aPoints) => {
              var xEmitter = new Emitters(aPoints);
              var xResult = (string)xMethod.Invoke(xEmitter, aPoints.Select(q => q.Value).ToArray());
              aCompiler.WriteLine(xResult);
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

  }
}
