using System;
using Spruce.Attribs;
using Spruce.Tokens;
using XSharp.Tokens;

namespace XSharp.x86.Emitters
{
    // Emitters does the actual translation from X# (via Spruce) to x86 (via Assemblers)
    public class BitwiseEmitters : Emitters
    {
        public BitwiseEmitters(Compiler aCompiler, x86.Assemblers.Assembler aAsm) : base(aCompiler, aAsm)
        {
        }

        [Emitter(typeof(Reg), typeof(OpBitwise), typeof(Const))]
        [Emitter(typeof(Reg), typeof(OpBitwise), typeof(Variable))]
        [Emitter(typeof(Reg08), typeof(OpBitwise), typeof(Reg08))]
        [Emitter(typeof(Reg08), typeof(OpBitwise), typeof(Int08u))]
        [Emitter(typeof(Reg16), typeof(OpBitwise), typeof(Reg16))]
        [Emitter(typeof(Reg16), typeof(OpBitwise), typeof(Int16u))]
        [Emitter(typeof(Reg32), typeof(OpBitwise), typeof(Reg32))]
        [Emitter(typeof(Reg32), typeof(OpBitwise), typeof(Int32u))]
        protected void BitwiseArithmetic(Register aRegister, string aOpBitwise, object aValue)
        {
            OpCode xOpCode = GetBitwiseOpCode(aOpBitwise);

            switch (aValue)
            {
                case Register xRegister:
                    Asm.Emit(xOpCode, aRegister, xRegister);
                    break;
                case uint xNum:
                    Asm.Emit(xOpCode, aRegister, xNum);
                    break;
                case string xString:
                    string xValue = Compiler.GetFullName($"Const_{xString}");
                    Asm.Emit(xOpCode, aRegister, xValue);
                    break;
                case x86.Params.Address xAddress:
                    xAddress.AddPrefix(Compiler.CurrentNamespace);
                    Asm.Emit(xOpCode, aRegister, xAddress);
                    break;
                default:
                    throw new Exception($"Unknown bitwise operand type.");
            }
        }

        [Emitter(typeof(Reg32), typeof(OpBitwise), typeof(VariableAddress))]
        protected void BitwiseArithmetic(Register aRegister, string aOpBitwise, string aValue)
        {
            OpCode xOpCode = GetBitwiseOpCode(aOpBitwise);
            string xValue = Compiler.GetFullName(aValue);
            Asm.Emit(xOpCode, aRegister, xValue);
        }

        [Emitter(typeof(Reg08), typeof(OpEquals), typeof(OpTilde), typeof(Reg08))]
        [Emitter(typeof(Reg16), typeof(OpEquals), typeof(OpTilde), typeof(Reg16))]
        [Emitter(typeof(Reg32), typeof(OpEquals), typeof(OpTilde), typeof(Reg32))]
        protected void BitwiseNot(Register aRegister, string aOpEquals, string aOpTilde, Register aSourceRegister)
        {
        }

        private static OpCode GetBitwiseOpCode(string aCompare)
        {
            switch (aCompare)
            {
                case "&":
                    return OpCode.And;
                case "|":
                    return OpCode.Or;
                case "~":
                    return OpCode.Not;
                default:
                    throw new Exception($"Unknown bitwise opcode '{aCompare}'");
            }
        }
    }
}
