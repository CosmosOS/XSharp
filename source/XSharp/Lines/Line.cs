using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace XSharp.Lines {
  public abstract class Line {
    public readonly Compiler Compiler;
    public readonly string RawText;

    public Line(Compiler aCompiler, string aLine) {
      Compiler = aCompiler;
      RawText = aLine;
    }

    public abstract void Emit();

    // This could be done with an registration scheme and each class searches on its own. 
    // However this would be a fair bit slower and this area is not expected to be expanded much,
    // nor is it intended for external expansion. Thus we have chosen for a bit of simplicity
    // and speed. If it needs expanded, its easily enough accomplished even with this method.
    public static Line New(Compiler aCompiler, string aLine) {
      Line xResult = null;
      if (aLine[0] == '/') {
        if (aLine.Length > 1) {
          char xChar2 = aLine[1];
          aLine = aLine.Substring(2).TrimStart();

          if (xChar2 == '/') {
            xResult = new Comment(aCompiler, aLine);

          } else if (xChar2 == '$') {
            xResult = new Directive(aCompiler, aLine);

          } else if (xChar2 == '!') {
            xResult = new Literal(aCompiler, aLine);
          }
        }
      }
      return xResult;
    }
  }
}
