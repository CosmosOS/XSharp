using System;
using Spruce.Attribs;
using XSharp.Tokens;
using XSharp.x86;
using XSharp.x86.Params;
using Reg = XSharp.Tokens.Reg;
using Reg08 = XSharp.Tokens.Reg08;
using Reg16 = XSharp.Tokens.Reg16;
using Reg32 = XSharp.Tokens.Reg32;

namespace XSharp.x86.Emitters
{
    /// <summary>
    /// The class that provides arithmetic for different types.
    /// </summary>
    /// <seealso cref="XSharp.x86.Emitters.Emitters" />
    public class Math : Emitters
    {
        public Math(Compiler aCompiler, x86.Assemblers.Assembler aAsm) : base(aCompiler, aAsm)
        {
        }

        [Emitter(typeof(Reg08), typeof(OpMath), typeof(Reg08))] // AH += BH
        [Emitter(typeof(Reg16), typeof(OpMath), typeof(Reg16))] // AX += BX
        [Emitter(typeof(Reg32), typeof(OpMath), typeof(Reg32))] // EAX += EBX
        protected void RegMathReg(Register aDestReg, string aOpMath, Register aSrcReg)
        {
            switch (aOpMath)
            {
                case "+=":
                    Asm.Emit(OpCode.Add, aDestReg, aSrcReg);
                    break;

                case "-=":
                    Asm.Emit(OpCode.Sub, aDestReg, aSrcReg);
                    break;

                case "*=":
                    Asm.Emit(OpCode.Mul, aDestReg, aSrcReg);
                    break;

                case "/=":
                    Asm.Emit(OpCode.Div, aDestReg, aSrcReg);
                    break;

                case "%=":
                    Asm.Emit(OpCode.Rem, aDestReg, aSrcReg);
                    break;

                default:
                    throw new Exception("Unsupported math operator");
            }
        }

        // Don't use Reg. This method ensures proper data sizes.
        [Emitter(typeof(Reg08), typeof(OpMath), typeof(Int08u))] // AH += 1
        [Emitter(typeof(Reg16), typeof(OpMath), typeof(Int16u))] // AX += 1
        [Emitter(typeof(Reg32), typeof(OpMath), typeof(Int32u))] // EAX += 1
        protected void RegMathNum(Register aDestReg, string aOpMath, object aVal)
        {
            switch (aOpMath)
            {
                case "+=":
                    Asm.Emit(OpCode.Add, aDestReg, aVal);
                    break;

                case "-=":
                    Asm.Emit(OpCode.Sub, aDestReg, aVal);
                    break;

                case "*=":
                    Asm.Emit(OpCode.Mul, aDestReg, aVal);
                    break;

                case "/=":
                    Asm.Emit(OpCode.Div, aDestReg, aVal);
                    break;

                case "%=":
                    Asm.Emit(OpCode.Rem, aDestReg, aVal);
                    break;

                default:
                    throw new Exception("Unsupported math operator");
            }
        }

        // AX += #Test
        [Emitter(typeof(Reg), typeof(OpMath), typeof(Const))]
        protected void RegMathConst(Register aReg, string aOpMath, string aVal)
        {
            string xConstName = Compiler.GetFullName($"Const_{aVal}");

            switch (aOpMath)
            {
                case "+=":
                    Asm.Emit(OpCode.Add, aReg, xConstName);
                    break;

                case "-=":
                    Asm.Emit(OpCode.Sub, aReg, xConstName);
                    break;

                case "*=":
                    Asm.Emit(OpCode.Mul, aReg, xConstName);
                    break;

                case "/=":
                    Asm.Emit(OpCode.Div, aReg, xConstName);
                    break;

                case "%=":
                    Asm.Emit(OpCode.Rem, aReg, xConstName);
                    break;

                default:
                    throw new Exception("Unsupported math operator");
            }
        }

        // EAX += .Varname
        [Emitter(typeof(Reg), typeof(OpMath), typeof(Variable))]
        protected void RegMathVar(Register aReg, string aOpMath, Address aVal)
        {
            // TODO: Do this better? Use Compiler.GetFullName() so things are consistent.
            aVal.AddPrefix(Compiler.CurrentNamespace);

            switch (aOpMath)
            {
                case "+=":
                    Asm.Emit(OpCode.Add, aReg, aReg.RegSize, aVal);
                    break;

                case "-=":
                    Asm.Emit(OpCode.Sub, aReg, aReg.RegSize, aVal);
                    break;

                case "*=":
                    Asm.Emit(OpCode.Mul, aReg, aReg.RegSize, aVal);
                    break;

                case "/=":
                    Asm.Emit(OpCode.Div, aReg, aReg.RegSize, aVal);
                    break;

                case "%=":
                    Asm.Emit(OpCode.Rem, aReg, aReg.RegSize, aVal);
                    break;

                default:
                    throw new Exception("Unsupported math operator");
            }
        }

        // AL += @.Varname
        [Emitter(typeof(Reg), typeof(OpMath), typeof(VariableAddress))]
        protected void RegMathVarAddress(Register aReg, string aOpMath, string aVal)
        {
            string xVariableName = Compiler.GetFullName(aVal);

            switch (aOpMath)
            {
                case "+=":
                    Asm.Emit(OpCode.Add, aReg, xVariableName);
                    break;

                case "-=":
                    Asm.Emit(OpCode.Sub, aReg, xVariableName);
                    break;

                case "*=":
                    Asm.Emit(OpCode.Mul, aReg, xVariableName);
                    break;

                case "/=":
                    Asm.Emit(OpCode.Div, aReg, xVariableName);
                    break;

                case "%=":
                    Asm.Emit(OpCode.Rem, aReg, xVariableName);
                    break;

                default:
                    throw new Exception("Unsupported math operator");
            }
        }
    }
}
