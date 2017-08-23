using System;
using System.Collections.Generic;
using System.Text;
using XSharp.Assembler;

namespace XSharp.Tokens {
  public abstract class TypedToken<T> : Token {
    public TypedToken(Parsers.Parser aParser) : base(aParser) { }

    protected abstract object IsMatch(T aValue);
    protected override object IsMatch(object aValue) {
      return IsMatch((T)aValue);
    }
  }
}
