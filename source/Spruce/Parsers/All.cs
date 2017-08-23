using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Linq;

namespace Spruce.Parsers {
  public class All : Parser {
    public override object Parse(string aText, ref int rStart) {
      string xResult = aText.Substring(rStart);
      rStart = aText.Length;
      return xResult;
    }
  }
}
