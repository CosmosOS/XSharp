using System;
using System.Collections.Generic;
using System.Text;
using Parsers = Spruce.Parsers;

namespace Spruce.Tokens {
  public abstract class TypedToken<T> : Token {
    public TypedToken(Parsers.Parser aParser) : base(aParser) { }

    protected abstract bool IsMatch(ref T rValue);
    protected override bool IsMatch(ref object rValue) {
      var xValue = (T)rValue;
      bool xResult = IsMatch(ref xValue);
      rValue = xValue;
      return xResult;
    }
  }
}
