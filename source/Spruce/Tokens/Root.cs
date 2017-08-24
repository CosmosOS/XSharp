using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Spruce;

namespace Spruce.Tokens {
    public class Root : Token {
        public Root(object aEmitter = null) : base(null) {
            if (aEmitter == null) {
                return;
            }

            // Load emitters to pattern list
            foreach (var xMethod in aEmitter.GetType().GetRuntimeMethods()) {
                var xAttrib = xMethod.GetCustomAttribute<Spruce.Attribs.Emitter>();
                if (xAttrib != null) {
                    AddPattern((List<CodePoint> aPoints) => {
                        if (xMethod.GetParameters().Length == 1) {
                            xMethod.Invoke(aEmitter, new object[] { aPoints.ToArray() });
                        } else {
                            xMethod.Invoke(aEmitter, aPoints.Select(q => q.Value).ToArray());
                        }
                    }, xAttrib.TokenTypes);
                }
            }
        }

        protected override bool IsMatch(ref object rValue) {
            return false;
        }

        public List<CodePoint> Parse(string aText) {
            // Important for end detection. Do not TrimStart, will goof up CodePoint indexes.
            aText = aText.TrimEnd();
            var xResult = new List<CodePoint>();
            int aPos = 0;
            Spruce.Tokens.Token xToken = this;

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
