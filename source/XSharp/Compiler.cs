using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using XSharp.x86.Assemblers;

namespace XSharp
{
    public class Compiler
    {
        protected Spruce.Tokens.Root mTokenMap;
        public readonly TextWriter Out;
        protected readonly x86.Assemblers.NASM mNASM;
        protected string Indent = "";
        public int LineNo { get; private set; }
        public bool EmitUserComments = true;
        public bool EmitSourceCode = true;

        public Compiler(TextWriter aOut)
        {
            Out = aOut;
            mNASM = new NASM(aOut);
            var xEmitters = new Emitters.Emitters(this, mNASM);
            mTokenMap = new Spruce.Tokens.Root(xEmitters);
        }

        public void WriteLine(string aText = "")
        {
            Out.WriteLine(Indent + aText);
        }

        public void Emit(TextReader aIn)
        {
            try
            {
                LineNo = 1;
                // Do not trim it here. We need spaces for colorizing
                // and also to keep indentation in the output.
                string xText = aIn.ReadLine();
                while (xText != null)
                {
                    int i = xText.Length - xText.TrimStart().Length;
                    mNASM.Indent = Indent = xText.Substring(0, i);

                    if (string.IsNullOrWhiteSpace(xText))
                    {
                        WriteLine();
                    }
                    else if (xText == "//END")
                    {
                        // Temp hack, remove in future
                        break;
                    }
                    else
                    {
                        var xCodePoints = mTokenMap.Parse(xText);
                        var xLastToken = xCodePoints.Last().Token;
                        if (EmitSourceCode && (xCodePoints[0].Token is Tokens.OpComment == false))
                        {
                            WriteLine("; " + xText.Trim());
                        }
                        xLastToken.Emitter(xCodePoints);
                    }

                    xText = aIn.ReadLine();
                    LineNo++;
                }
            }
            catch (Exception e)
            {
                throw new Exception("Generation error on line " + LineNo, e);
            }
        }
    }
}
