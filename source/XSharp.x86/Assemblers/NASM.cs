using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using XSharp.x86.Params;

namespace XSharp.x86.Assemblers
{
    public class NASM : Assembler
    {
        protected readonly Map mMap;
        public string Indent = "";
        protected readonly TextWriter mOut;

        public NASM(TextWriter aOut)
        {
            mOut = aOut;

            mMap = new Map();
            Add(OpCode.Mov, "{0}, 0x{1:X}", typeof(Reg08), typeof(i08u));
            Add(OpCode.Mov, "{0}, 0x{1:X}", typeof(Reg16), typeof(i16u));
            Add(OpCode.Mov, "{0}, 0x{1:X}", typeof(Reg32), typeof(i32u));
            Add(OpCode.NOP);
            Add(OpCode.RET);
            Add(OpCode.Pushad);
            Add(OpCode.Popad);
        }

        protected void Add(OpCode aOpCode, string aOutput = null, params Type[] aParamTypes)
        {
            Param.ActionDelegate xAction;
            if (aOutput == null)
            {
                xAction = (object[] aValues) =>
                {
                    mOut.WriteLine();
                };
            }
            else
            {
                xAction = (object[] aValues) =>
                {
                    // Can be done with a single call to .WriteLine but makes
                    // debugging far more difficult.
                    string xOut = string.Format(aOutput, aValues);
                    mOut.WriteLine(xOut);
                };
            }
            mMap.Add(xAction, aOpCode, aParamTypes);
        }

        public override void Emit(OpCode aOp, params object[] aParams)
        {
            mOut.Write(Indent + aOp + " ");
            mMap.Execute(aOp, aParams);
        }
    }
}
