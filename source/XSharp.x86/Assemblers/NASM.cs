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

            // Add in alphabetical order from here

            Add(OpCode.Dec, "{0}", typeof(Reg08));
            Add(OpCode.Dec, "{0}", typeof(Reg16));
            Add(OpCode.Dec, "{0}", typeof(Reg32));

            Add(OpCode.In, "{0}, {1}", typeof(Reg08), typeof(Reg16));
            Add(OpCode.In, "{0}, {1}", typeof(Reg16), typeof(Reg16));
            Add(OpCode.In, "{0}, {1}", typeof(Reg32), typeof(Reg16));
            Add(OpCode.In, "{0}, 0x{1:X}", typeof(Reg08), typeof(i08u));
            Add(OpCode.In, "{0}, 0x{1:X}", typeof(Reg16), typeof(i08u));
            Add(OpCode.In, "{0}, 0x{1:X}", typeof(Reg32), typeof(i08u));

            Add(OpCode.Inc, "{0}", typeof(Reg08));
            Add(OpCode.Inc, "{0}", typeof(Reg16));
            Add(OpCode.Inc, "{0}", typeof(Reg32));

            Add(OpCode.Mov, "{0}, 0x{1:X}", typeof(Reg08), typeof(i08u));
            Add(OpCode.Mov, "{0}, 0x{1:X}", typeof(Reg16), typeof(i16u));
            Add(OpCode.Mov, "{0}, 0x{1:X}", typeof(Reg32), typeof(i32u));

            Add(OpCode.NOP);

            Add(OpCode.Out, "{0}, {1}", typeof(Reg16), typeof(Reg08));
            Add(OpCode.Out, "{0}, {1}", typeof(Reg16), typeof(Reg16));
            Add(OpCode.Out, "{0}, {1}", typeof(Reg16), typeof(Reg32));
            Add(OpCode.Out, "0x{0:X}, {1}", typeof(i08u), typeof(Reg08));
            Add(OpCode.Out, "0x{0:X}, {1}", typeof(i08u), typeof(Reg16));
            Add(OpCode.Out, "0x{0:X}, {1}", typeof(i08u), typeof(Reg32));

            Add(OpCode.Push, "{0}", typeof(Reg08));
            Add(OpCode.Push, "{0}", typeof(Reg16));
            Add(OpCode.Push, "{0}", typeof(Reg32));
            Add(OpCode.Push, "0x{0:X}", typeof(i08u));
            Add(OpCode.Push, "0x{0:X}", typeof(i16u));
            Add(OpCode.Push, "0x{0:X}", typeof(i32u));
            Add(OpCode.Push, "{0}", typeof(SingleWord));

            Add(OpCode.PushAD);

            Add(OpCode.Pop, "{0}", typeof(Reg08));
            Add(OpCode.Pop, "{0}", typeof(Reg16));
            Add(OpCode.Pop, "{0}", typeof(Reg32));
            Add(OpCode.Pop, "0x{0:X}", typeof(i08u));
            Add(OpCode.Pop, "0x{0:X}", typeof(i16u));
            Add(OpCode.Pop, "0x{0:X}", typeof(i32u));
            Add(OpCode.Pop, "{0}", typeof(SingleWord));

            Add(OpCode.PopAD);
            Add(OpCode.Ret);

            Add(OpCode.Test, "{0}, 0x{1:X}", typeof(Reg08), typeof(i08u));
            Add(OpCode.Test, "{0}, 0x{1:X}", typeof(Reg16), typeof(i16u));
            Add(OpCode.Test, "{0}, 0x{1:X}", typeof(Reg32), typeof(i32u));
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
