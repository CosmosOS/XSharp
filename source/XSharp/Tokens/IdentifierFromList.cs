using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XSharp.Tokens {
  public abstract class IdentifierFromList : Token {
    protected string[] mList;

    protected IdentifierFromList() {
      mParser = Parsers.Parsers.IdentifierUpper;
      mList = GetList();
    }

    protected abstract string[] GetList();

    protected override object IsMatch(object aValue) {
      if (aValue is string && mList.Contains((string)aValue)) {
        return aValue;
      }
      return null;
    }
  }
}
