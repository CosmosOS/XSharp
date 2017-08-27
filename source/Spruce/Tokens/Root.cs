using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using Spruce;

namespace Spruce.Tokens {
    public class Root : Token {
        public Root(object aEmitter = null) : base() {
            if (aEmitter == null) {
                return;
            }

            // Load emitters to pattern list
            foreach (var xMethod in aEmitter.GetType().GetRuntimeMethods()) {
                foreach (var xAttrib in xMethod.GetCustomAttributes<Attribs.Emitter>()) {
                    Token.Action xAction;
                    var xParams = xMethod.GetParameters();
                    if (xParams.Length == 0) {
                        xAction = (List<CodePoint> aPoints) => {
                            xMethod.Invoke(aEmitter, null);
                        };
                    } else { 
                        var xParam = xParams[0];
                        if (xParam.ParameterType.IsGenericType) {
                            xAction = (List<CodePoint> aPoints) => {
                                xMethod.Invoke(aEmitter, new object[] { aPoints });
                            };
                        } else {
                            xAction = (List<CodePoint> aPoints) => {
                                xMethod.Invoke(aEmitter, aPoints.Select(q => q.Value).ToArray());
                            };
                        }
                    }
                    AddEmitter(xAction, xAttrib.TokenTypes);
                }
            }
        }

        protected override object Check(string aText) {
            throw new Exception("Check not valid on Root.");
        }
        protected override object Parse(string aText, ref int rStart) {
            throw new Exception("Parse not valid on Root.");
        }

        public List<CodePoint> Parse(string aText) {
            try {
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
                    xResult.Add(xCP);
                    xToken = xCP.Token;
                }
                if (aPos < aText.Length) {
                    throw new Exception("Text on line beyond end of parse.");
                }

                return xResult;
            } catch (Exception ex) {
                throw new Exception("Parse Error." + aText, ex);
            }
        }
    }
}
