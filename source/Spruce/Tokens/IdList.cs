using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Spruce.Tokens {
  public abstract class IdList : ID {
    protected string[] mList;

    protected IdList(bool aUpper = true) : base(null, aUpper) {
      mList = GetList();
    }

    protected abstract string[] GetList();

    protected override bool IsMatch(ref string rValue) {
      return mList.Contains(rValue);
    }
  }
}
