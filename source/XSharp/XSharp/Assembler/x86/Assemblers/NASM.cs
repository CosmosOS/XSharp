using System;
using System.IO;

using XSharp.x86.Params;

namespace XSharp.x86.Assemblers
{
    public class NASM : Assembler
    {
        public NASM(TextWriter aOut)
        {
            Out = aOut;

            Map = new Map();

            // Add in alphabetical order from here

            Add(OpCode.Add, "{0}, {1}", typeof(Reg08), typeof(Reg08));
            Add(OpCode.Add, "{0}, {1}", typeof(Reg16), typeof(Reg16));
            Add(OpCode.Add, "{0}, {1}", typeof(Reg32), typeof(Reg32));
            Add(OpCode.Add, "{0}, 0x{1:X}", typeof(Reg08), typeof(i08u));
            Add(OpCode.Add, "{0}, 0x{1:X}", typeof(Reg16), typeof(i16u));
            Add(OpCode.Add, "{0}, 0x{1:X}", typeof(Reg32), typeof(i32u));

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

            Add(OpCode.Mov, "{0}, {1}", typeof(Reg08), typeof(Reg08));
            Add(OpCode.Mov, "{0}, {1}", typeof(Reg16), typeof(Reg16));
            Add(OpCode.Mov, "{0}, {1}", typeof(Reg32), typeof(Reg32));
            Add(OpCode.Mov, "{0}, 0x{1:X}", typeof(Reg08), typeof(i08u));
            Add(OpCode.Mov, "{0}, 0x{1:X}", typeof(Reg16), typeof(i16u));
            Add(OpCode.Mov, "{0}, 0x{1:X}", typeof(Reg32), typeof(i32u));
            Add(OpCode.Mov, "{0}, {1} {2}", typeof(Reg08), typeof(Size), typeof(RegisterAddress));
            Add(OpCode.Mov, "{0}, {1} {2}", typeof(Reg16), typeof(Size), typeof(RegisterAddress));
            Add(OpCode.Mov, "{0}, {1} {2}", typeof(Reg32), typeof(Size), typeof(RegisterAddress));
            Add(OpCode.Mov, "{0}, {1} {2}", typeof(Reg08), typeof(Size), typeof(MemoryAddress));
            Add(OpCode.Mov, "{0}, {1} {2}", typeof(Reg16), typeof(Size), typeof(MemoryAddress));
            Add(OpCode.Mov, "{0}, {1} {2}", typeof(Reg32), typeof(Size), typeof(MemoryAddress));
            Add(OpCode.Mov, "{0} {1}, {2}", typeof(Size), typeof(RegisterAddress), typeof(Reg08));
            Add(OpCode.Mov, "{0} {1}, {2}", typeof(Size), typeof(RegisterAddress), typeof(Reg16));
            Add(OpCode.Mov, "{0} {1}, {2}", typeof(Size), typeof(RegisterAddress), typeof(Reg32));
            Add(OpCode.Mov, "{0} {1}, {2}", typeof(Size), typeof(RegisterAddress), typeof(Identifier));
            Add(OpCode.Mov, "{0} {1}, 0x{2:X}", typeof(Size), typeof(RegisterAddress), typeof(i32u));
            Add(OpCode.Mov, "{0} {1}, 0x{2:X}", typeof(Size), typeof(MemoryAddress), typeof(i32u));
            Add(OpCode.Mov, "{0} {1}, {2}", typeof(Size), typeof(MemoryAddress), typeof(Reg08));
            Add(OpCode.Mov, "{0} {1}, {2}", typeof(Size), typeof(MemoryAddress), typeof(Reg16));
            Add(OpCode.Mov, "{0} {1}, {2}", typeof(Size), typeof(MemoryAddress), typeof(Reg32));
            Add(OpCode.Mov, "{0}, {1}", typeof(MemoryAddress), typeof(Identifier));
            Add(OpCode.Mov, "{0}, {1}", typeof(Reg08), typeof(Identifier));
            Add(OpCode.Mov, "{0}, {1}", typeof(Reg16), typeof(Identifier));
            Add(OpCode.Mov, "{0}, {1}", typeof(Reg32), typeof(Identifier));

            Add(OpCode.Mul, "{0}, {1}", typeof(Reg08), typeof(Reg08));
            Add(OpCode.Mul, "{0}, {1}", typeof(Reg16), typeof(Reg16));
            Add(OpCode.Mul, "{0}, {1}", typeof(Reg32), typeof(Reg32));
            Add(OpCode.Mul, "{0}, 0x{1:X}", typeof(Reg08), typeof(i08u));
            Add(OpCode.Mul, "{0}, 0x{1:X}", typeof(Reg16), typeof(i16u));
            Add(OpCode.Mul, "{0}, 0x{1:X}", typeof(Reg32), typeof(i32u));

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
            Add(OpCode.Push, "{0}", typeof(Identifier));
            Add(OpCode.Push, "{0}", typeof(MemoryAddress));
            Add(OpCode.Push, "{0}", typeof(RegisterAddress));

            Add(OpCode.PushAD);

            Add(OpCode.Pop, "{0}", typeof(Reg08));
            Add(OpCode.Pop, "{0}", typeof(Reg16));
            Add(OpCode.Pop, "{0}", typeof(Reg32));
            Add(OpCode.Pop, "0x{0:X}", typeof(i08u));
            Add(OpCode.Pop, "0x{0:X}", typeof(i16u));
            Add(OpCode.Pop, "0x{0:X}", typeof(i32u));
            Add(OpCode.Pop, "{0}", typeof(Identifier));
            Add(OpCode.Pop, "{0}", typeof(MemoryAddress));
            Add(OpCode.Pop, "{0}", typeof(RegisterAddress));

            Add(OpCode.PopAD);

            Add(OpCode.Rem, "{0}, {1}", typeof(Reg08), typeof(Reg08));
            Add(OpCode.Rem, "{0}, {1}", typeof(Reg16), typeof(Reg16));
            Add(OpCode.Rem, "{0}, {1}", typeof(Reg32), typeof(Reg32));
            Add(OpCode.Rem, "{0}, 0x{1:X}", typeof(Reg08), typeof(i08u));
            Add(OpCode.Rem, "{0}, 0x{1:X}", typeof(Reg16), typeof(i16u));
            Add(OpCode.Rem, "{0}, 0x{1:X}", typeof(Reg32), typeof(i32u));

            Add(OpCode.Ret);

            Add(OpCode.Rol, "{0}, 0x{1:X}", typeof(Reg32), typeof(i08u));

            Add(OpCode.Ror, "{0}, 0x{1:X}", typeof(Reg32), typeof(i08u));

            Add(OpCode.Shl, "{0}, 0x{1:X}", typeof(Reg32), typeof(i08u));

            Add(OpCode.Shr, "{0}, 0x{1:X}", typeof(Reg32), typeof(i08u));

            Add(OpCode.Sub, "{0}, {1}", typeof(Reg08), typeof(Reg08));
            Add(OpCode.Sub, "{0}, {1}", typeof(Reg16), typeof(Reg16));
            Add(OpCode.Sub, "{0}, {1}", typeof(Reg32), typeof(Reg32));
            Add(OpCode.Sub, "{0}, 0x{1:X}", typeof(Reg08), typeof(i08u));
            Add(OpCode.Sub, "{0}, 0x{1:X}", typeof(Reg16), typeof(i16u));
            Add(OpCode.Sub, "{0}, 0x{1:X}", typeof(Reg32), typeof(i32u));

            Add(OpCode.Test, "{0}, 0x{1:X}", typeof(Reg08), typeof(i08u));
            Add(OpCode.Test, "{0}, 0x{1:X}", typeof(Reg16), typeof(i16u));
            Add(OpCode.Test, "{0}, 0x{1:X}", typeof(Reg32), typeof(i32u));
        }

        protected Map Map { get; }

        protected TextWriter Out { get; }

        public string Indent { get; set; }

        protected void Add(OpCode aOpCode, string aOutput = null, params Type[] aParamTypes)
        {
            Action<object[]> xAction;
            if (aOutput == null)
            {
                xAction = (object[] aValues) =>
                {
                    Out.WriteLine();
                };
            }
            else
            {
                xAction = (object[] aValues) =>
                {
                    // Can be done with a single call to .WriteLine but makes
                    // debugging far more difficult.
                    string xOut = String.Format(aOutput, aValues);
                    Out.WriteLine(xOut);
                };
            }
            Map.Add(xAction, aOpCode, aParamTypes);
        }

        public override void Emit(OpCode aOp, params object[] aParams)
        {
            Out.Write(Indent + aOp + " ");
            Map.Execute(aOp, aParams);
        }
    }
}
