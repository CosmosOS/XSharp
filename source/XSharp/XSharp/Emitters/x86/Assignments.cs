using System.Text;
using Spruce.Attribs;
using Spruce.Tokens;
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
    /// The class that provides assignments for different types.
    /// </summary>
    /// <seealso cref="XSharp.x86.Emitters.Emitters" />
    public class Assignments : Emitters
    {
        public Assignments(Compiler aCompiler, x86.Assemblers.Assembler aAsm) : base(aCompiler, aAsm)
        {
        }

        // EAX = [EBX]
        [Emitter(typeof(Reg), typeof(OpEquals), typeof(OpOpenBracket), typeof(Reg), typeof(OpCloseBracket))]
        protected void MemoryAssignToReg(Register aRegister, string aOpEquals, string aOpOpenBracket, Register aSourceRegister, string aOpCloseBracket)
        {
            Asm.Emit(OpCode.Mov, aRegister, aRegister.RegSize, new Address(aSourceRegister));
        }

        [Emitter(typeof(Reg08), typeof(OpEquals), typeof(Reg08))] // AH = BH
        [Emitter(typeof(Reg16), typeof(OpEquals), typeof(Reg16))] // AX = BX
        [Emitter(typeof(Reg32), typeof(OpEquals), typeof(Reg32))] // EAX = EBX
        protected void RegAssigReg(Register aDestReg, string aEquals, Register aSrcReg)
        {
            Asm.Emit(OpCode.Mov, aDestReg, aSrcReg);
        }

        // [EAX] = EAX
        [Emitter(typeof(OpOpenBracket), typeof(Reg), typeof(OpCloseBracket), typeof(OpEquals), typeof(Reg))]
        protected void RegAssignToMemory(string aOpOpenBracket, Register aTargetRegisterRoot, string aOpCloseBracket, string aOpEquals, Register source)
        {
            Asm.Emit(OpCode.Mov, source.RegSize, new Address(aTargetRegisterRoot), source);
        }

        // [EAX] = .varname
        [Emitter(typeof(OpOpenBracket), typeof(Reg), typeof(OpCloseBracket), typeof(OpEquals), typeof(Variable))]
        protected void VariableAssignToMemory(string aOpOpenBracket, Register aTargetRegisterRoot, string aOpCloseBracket, string aOpEquals, Address source)
        {
            source.AddPrefix(Compiler.CurrentNamespace);
            Asm.Emit(OpCode.Mov, "dword", new Address(aTargetRegisterRoot), source);
        }

        // [EAX] = 0x10
        [Emitter(typeof(OpOpenBracket), typeof(Reg), typeof(OpCloseBracket), typeof(OpEquals), typeof(Int32u))]
        protected void IntegerAssignToMemory(string aOpOpenBracket, Register aTargetRegisterRoot, string aOpCloseBracket, string aOpEquals, object source)
        {
            Asm.Emit(OpCode.Mov, "dword", new Address(aTargetRegisterRoot), source);
        }

        // Don't use Reg. This method ensures proper data sizes.
        // This could be combined with RegAssignReg by using object type for last arg, but this is a bit cleaner to separate.
        [Emitter(typeof(Reg08), typeof(OpEquals), typeof(Int08u))] // AH = 0
        [Emitter(typeof(Reg16), typeof(OpEquals), typeof(Int16u))] // AX = 0
        [Emitter(typeof(Reg32), typeof(OpEquals), typeof(Int32u))] // EAX = 0
        protected void RegAssignNum(Register aDestReg, string aEquals, object aVal)
        {
            Asm.Emit(OpCode.Mov, aDestReg, aVal);
        }

        [Emitter(typeof(Reg), typeof(OpEquals), typeof(CharacterLiteral))]
        protected void RegAssignStringListeral(Register aReg, string aEquals, string aVal)
        {
            // This will only ever be a single character
            byte[] chars = Encoding.ASCII.GetBytes(aVal);
            Asm.Emit(OpCode.Mov, aReg, chars[0]);
        }

        // AX = #Test
        [Emitter(typeof(Reg), typeof(OpEquals), typeof(Const))]
        protected void RegAssignConst(Register aReg, string aEquals, string aVal)
        {
            string xValue = Compiler.GetFullName($"Const_{aVal}");
            Asm.Emit(OpCode.Mov, aReg, xValue);
        }

        // EAX = .Varname
        [Emitter(typeof(Reg), typeof(OpEquals), typeof(Variable))]
        protected void RegAssignVar(Register aReg, string aEquals, Address aVal)
        {
            // TODO: Do this better? Use Compiler.GetFullName() so things are consistent.
            aVal.AddPrefix(Compiler.CurrentNamespace);
            Asm.Emit(OpCode.Mov, aReg, aReg.RegSize, aVal);
        }

        // AL = @.varname
        [Emitter(typeof(Reg), typeof(OpEquals), typeof(VariableAddress))]
        protected void RegAssignVarAddress(Register aReg, string aEquals, string aVal)
        {
            string xValue = Compiler.GetFullName(aVal);
            Asm.Emit(OpCode.Mov, aReg, xValue);
        }

        // ESI = [EBP-1] => mov ESI, dword [EBP - 1]
        [Emitter(typeof(Reg), typeof(OpEquals), typeof(OpOpenBracket), typeof(Reg08), typeof(OpPlus), typeof(Int08u), typeof(OpCloseBracket))]
        [Emitter(typeof(Reg), typeof(OpEquals), typeof(OpOpenBracket), typeof(Reg16), typeof(OpPlus), typeof(Int16u), typeof(OpCloseBracket))]
        [Emitter(typeof(Reg), typeof(OpEquals), typeof(OpOpenBracket), typeof(Reg32), typeof(OpPlus), typeof(Int32u), typeof(OpCloseBracket))]
        [Emitter(typeof(Reg), typeof(OpEquals), typeof(OpOpenBracket), typeof(Reg08), typeof(OpMinus), typeof(Int08u), typeof(OpCloseBracket))]
        [Emitter(typeof(Reg), typeof(OpEquals), typeof(OpOpenBracket), typeof(Reg16), typeof(OpMinus), typeof(Int16u), typeof(OpCloseBracket))]
        [Emitter(typeof(Reg), typeof(OpEquals), typeof(OpOpenBracket), typeof(Reg32), typeof(OpMinus), typeof(Int32u), typeof(OpCloseBracket))]
        protected void MemoryAssignToReg(Register aDestReg, string aOpEquals, string aOpOpen, Register aSrc, string aOp, object aOffset, string aOpClose)
        {
            Asm.Emit(OpCode.Mov, aDestReg, aDestReg.RegSize, new Address(aSrc, aOffset, aOp == "-"));
        }

        // [AX + 12] = EAX => mov dword [AX + 12], EAX
        [Emitter(typeof(OpOpenBracket), typeof(Reg08), typeof(OpPlus), typeof(Int08u), typeof(OpCloseBracket), typeof(OpEquals), typeof(Reg))]
        [Emitter(typeof(OpOpenBracket), typeof(Reg16), typeof(OpPlus), typeof(Int16u), typeof(OpCloseBracket), typeof(OpEquals), typeof(Reg))]
        [Emitter(typeof(OpOpenBracket), typeof(Reg32), typeof(OpPlus), typeof(Int32u), typeof(OpCloseBracket), typeof(OpEquals), typeof(Reg))]
        [Emitter(typeof(OpOpenBracket), typeof(Reg08), typeof(OpMinus), typeof(Int08u), typeof(OpCloseBracket), typeof(OpEquals), typeof(Reg))]
        [Emitter(typeof(OpOpenBracket), typeof(Reg16), typeof(OpMinus), typeof(Int16u), typeof(OpCloseBracket), typeof(OpEquals), typeof(Reg))]
        [Emitter(typeof(OpOpenBracket), typeof(Reg32), typeof(OpMinus), typeof(Int32u), typeof(OpCloseBracket), typeof(OpEquals), typeof(Reg))]
        protected void RegAssignToMemory(string aOpOpenBracket, Register aTargetRegisterRoot, string aOpOperator, object aOffset, string aOpCloseBracket, string aOpEquals, Register source)
        {
            Asm.Emit(OpCode.Mov, source.RegSize, new Address(aTargetRegisterRoot, aOffset, aOpOperator == "-"), source);
        }

        // .v1 = 1 => mov dword [v1], 0x1
        [Emitter(typeof(Variable), typeof(OpEquals), typeof(Int32u))]
        [Emitter(typeof(Variable), typeof(OpEquals), typeof(Reg))]
        [Emitter(typeof(Variable), typeof(OpEquals), typeof(Const))]
        protected void VariableAssignment(Address aVariableName, string aOpEquals, object aValue)
        {
            // TODO: Do this better? Use Compiler.GetFullName() so things are consistent.
            aVariableName.AddPrefix(Compiler.CurrentNamespace);

            string size;
            switch (aValue)
            {
                case uint _:
                    size = "dword";
                    Asm.Emit(OpCode.Mov, size, aVariableName, aValue);
                    break;

                case Register aValueReg:
                    size = aValueReg.RegSize;
                    Asm.Emit(OpCode.Mov, size, aVariableName, aValue);
                    break;

                case string _:
                    //TODO: verify this
                    aValue = Compiler.GetFullName($"Const_{aValue}");
                    Asm.Emit(OpCode.Mov, aVariableName, aValue);
                    break;
            }
        }
    }
}
