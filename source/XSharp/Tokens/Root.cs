using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace XSharp.Tokens {
  public class Root : Token {
    public Root(Type aEmitterType) {
      // Load emitters to pattern list
      foreach (var xMethod in typeof(Emitters).GetRuntimeMethods()) {
        var xAttrib = xMethod.GetCustomAttribute<EmitterAttribute>();
        if (xAttrib != null) {
          AddPattern(
            (Compiler aCompiler, List<CodePoint> aPoints) => {
              var xEmitter = Activator.CreateInstance(aEmitterType, aPoints);
              var xResult = (string)xMethod.Invoke(xEmitter, aPoints.Select(q => q.Value).ToArray());
              aCompiler.WriteLine(xResult);
            }
            , xAttrib.TokenTypes);
        }
      }
    }

    protected override object IsMatch(object aValue) {
      return null;
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
