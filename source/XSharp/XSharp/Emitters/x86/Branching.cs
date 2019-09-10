using System;
using Spruce.Attribs;
using Spruce.Tokens;
using XSharp.Tokens;
using XSharp.x86.Params;
using Identifier = XSharp.Tokens.Identifier;
using Reg = XSharp.Tokens.Reg;
using Reg08 = XSharp.Tokens.Reg08;
using Reg16 = XSharp.Tokens.Reg16;
using Reg32 = XSharp.Tokens.Reg32;
using Size = XSharp.Tokens.Size;

namespace XSharp.x86.Emitters
{
    /// <summary>
    /// Class that processes conditional branches for X#.
    /// </summary>
    /// <seealso cref="XSharp.x86.Emitters.Emitters" />
    public class Branching : Emitters
    {
        public Branching(Compiler aCompiler, x86.Assemblers.Assembler aAsm) : base(aCompiler, aAsm)
        {
        }

        #region if return
        [Emitter(typeof(If), typeof(Reg), typeof(OpCompare), typeof(Reg), typeof(Return))]
        [Emitter(typeof(If), typeof(Reg08), typeof(OpCompare), typeof(Int08u), typeof(Return))]
        [Emitter(typeof(If), typeof(Reg16), typeof(OpCompare), typeof(Int16u), typeof(Return))]
        [Emitter(typeof(If), typeof(Reg32), typeof(OpCompare), typeof(Int32u), typeof(Return))]
        protected void IfRegisterConditionRegisterReturn(string aOpIf, Register aRegister, string aOpCompare, object aValue, object aOpReturn)
        {
            var xJumpOpCode = GetJumpOpCode(aOpCompare);
            Asm.Emit(OpCode.Cmp, aRegister, aValue);
            Asm.Emit(xJumpOpCode, Compiler.CurrentFunctionExitLabel);
        }

        [Emitter(typeof(If), typeof(Reg), typeof(OpCompare), typeof(Const), typeof(Return))]
        protected void IfRegisterConditionConstReturn(string aOpIf, Register aRegister, string aOpCompare, string aValue, object aOpReturn)
        {
            var xJumpOpCode = GetJumpOpCode(aOpCompare);
            string xValue = Compiler.GetFullName($"Const_{aValue}");
            Asm.Emit(OpCode.Cmp, aRegister, xValue);
            Asm.Emit(xJumpOpCode, Compiler.CurrentFunctionExitLabel);
        }

        [Emitter(typeof(If), typeof(Reg), typeof(OpCompare), typeof(Variable), typeof(Return))]
        protected void IfRegisterConditionVariableReturn(string aOpIf, Register aRegister, string aOpCompare, Address aValue, object aOpReturn)
        {
            var xJumpOpCode = GetJumpOpCode(aOpCompare);
            aValue.AddPrefix(Compiler.CurrentNamespace);
            Asm.Emit(OpCode.Cmp, aRegister, aValue);
            Asm.Emit(xJumpOpCode, Compiler.CurrentFunctionExitLabel);
        }

        [Emitter(typeof(If), typeof(Size), typeof(Reg), typeof(OpCompare), typeof(Variable), typeof(Return))]
        protected void IfSizeRegisterConditionVariableReturn(string aOpIf, string aSize, Register aRegister, string aOpCompare, Address aValue, object aOpReturn)
        {
            var xJumpOpCode = GetJumpOpCode(aOpCompare);
            aValue.AddPrefix(Compiler.CurrentNamespace);
            Asm.Emit(OpCode.Cmp, aSize, aRegister, aValue);
            Asm.Emit(xJumpOpCode, Compiler.CurrentFunctionExitLabel);
        }

        [Emitter(typeof(If), typeof(Reg), typeof(OpCompare), typeof(VariableAddress), typeof(Return))]
        protected void IfRegisterConditionVariableAddressReturn(string aOpIf, Register aRegister, string aOpCompare, string aValue, object aOpReturn)
        {
            var xJumpOpCode = GetJumpOpCode(aOpCompare);
            string xValue = Compiler.GetFullName(aValue);
            Asm.Emit(OpCode.Cmp, aRegister, xValue);
            Asm.Emit(xJumpOpCode, Compiler.CurrentFunctionExitLabel);
        }
        #endregion

        #region if {
        [Emitter(typeof(If), typeof(Reg), typeof(OpCompare), typeof(Reg), typeof(OpOpenBrace))]
        protected void IfRegisterConditionRegister(string aOpIf, Register aLeftRegister, string aOpCompare, Register aRightRegister, object aOpOpenBrace)
        {
            Compiler.Blocks.StartBlock(Compiler.BlockType.If);
            var xJumpOpCode = GetOppositeJumpOpCode(aOpCompare);
            Asm.Emit(OpCode.Cmp, aLeftRegister, aRightRegister);
            Asm.Emit(xJumpOpCode, Compiler.Blocks.EndBlockLabel);
        }

        [Emitter(typeof(If), typeof(Reg08), typeof(OpCompare), typeof(Int08u), typeof(OpOpenBrace))]
        [Emitter(typeof(If), typeof(Reg16), typeof(OpCompare), typeof(Int16u), typeof(OpOpenBrace))]
        [Emitter(typeof(If), typeof(Reg32), typeof(OpCompare), typeof(Int32u), typeof(OpOpenBrace))]
        protected void IfRegisterConditionConst(string aOpIf, Register aRegister, string aOpCompare, object aValue, object aOpOpenBrace)
        {
            Compiler.Blocks.StartBlock(Compiler.BlockType.If);
            var xJumpOpCode = GetOppositeJumpOpCode(aOpCompare);
            Asm.Emit(OpCode.Cmp, aRegister, aValue);
            Asm.Emit(xJumpOpCode, Compiler.Blocks.EndBlockLabel);
        }

        [Emitter(typeof(If), typeof(Reg), typeof(OpCompare), typeof(Const), typeof(OpOpenBrace))]
        protected void IfRegisterConditionConst(string aOpIf, Register aRegister, string aOpCompare, string aValue, object aOpOpenBrace)
        {
            Compiler.Blocks.StartBlock(Compiler.BlockType.If);
            var xJumpOpCode = GetOppositeJumpOpCode(aOpCompare);
            string xValue = Compiler.GetFullName($"Const_{aValue}");
            Asm.Emit(OpCode.Cmp, aRegister, xValue);
            Asm.Emit(xJumpOpCode, Compiler.Blocks.EndBlockLabel);
        }

        [Emitter(typeof(If), typeof(Reg), typeof(OpCompare), typeof(Variable), typeof(OpOpenBrace))]
        protected void IfRegisterConditionVariable(string aOpIf, Register aRegister, string aOpCompare, Address aValue, object aOpOpenBrace)
        {
            Compiler.Blocks.StartBlock(Compiler.BlockType.If);
            var xJumpOpCode = GetOppositeJumpOpCode(aOpCompare);
            aValue.AddPrefix(Compiler.CurrentNamespace);
            Asm.Emit(OpCode.Cmp, aRegister, aValue);
            Asm.Emit(xJumpOpCode, Compiler.Blocks.EndBlockLabel);
        }

        [Emitter(typeof(If), typeof(Reg), typeof(OpCompare), typeof(VariableAddress), typeof(OpOpenBrace))]
        protected void IfRegisterConditionVariableAddress(string aOpIf, Register aRegister, string aOpCompare, string aValue, object aOpOpenBrace)
        {
            Compiler.Blocks.StartBlock(Compiler.BlockType.If);
            var xJumpOpCode = GetOppositeJumpOpCode(aOpCompare);
            string xValue = Compiler.GetFullName(aValue);
            Asm.Emit(OpCode.Cmp, aRegister, xValue);
            Asm.Emit(xJumpOpCode, Compiler.Blocks.EndBlockLabel);
        }
        #endregion

        #region if goto
        [Emitter(typeof(If), typeof(Reg), typeof(OpCompare), typeof(Reg), typeof(GotoKeyword), typeof(Identifier))]
        protected void IfRegisterConditionRegisterGoto(string aOpIf, Register aLeftRegister, string aOpCompare, Register aRightRegister, string aGotoKeyword, string aLabel)
        {
            var xJumpOpCode = GetJumpOpCode(aOpCompare);
            Asm.Emit(OpCode.Cmp, aLeftRegister, aRightRegister);
            string xLabel = Compiler.GetFullName(aLabel, true);
            Asm.Emit(xJumpOpCode, xLabel);
        }

        [Emitter(typeof(If), typeof(Reg08), typeof(OpCompare), typeof(Int08u), typeof(GotoKeyword), typeof(Identifier))]
        [Emitter(typeof(If), typeof(Reg16), typeof(OpCompare), typeof(Int16u), typeof(GotoKeyword), typeof(Identifier))]
        [Emitter(typeof(If), typeof(Reg32), typeof(OpCompare), typeof(Int32u), typeof(GotoKeyword), typeof(Identifier))]
        protected void IfRegisterConditionIntGoto(string aOpIf, Register aRegister, string aOpCompare, object aValue, string aGotoKeyword, string aLabel)
        {
            var xJumpOpCode = GetJumpOpCode(aOpCompare);
            Asm.Emit(OpCode.Cmp, aRegister, aValue);
            string xLabel = Compiler.GetFullName(aLabel, true);
            Asm.Emit(xJumpOpCode, xLabel);
        }

        [Emitter(typeof(If), typeof(Reg), typeof(OpCompare), typeof(Const), typeof(GotoKeyword), typeof(Identifier))]
        protected void IfRegisterConditionConstGoto(string aOpIf, Register aRegister, string aOpCompare, string aValue, string aGotoKeyword, string aLabel)
        {
            var xJumpOpCode = GetJumpOpCode(aOpCompare);
            string xValue = Compiler.GetFullName($"Const_{aValue}");
            Asm.Emit(OpCode.Cmp, aRegister, xValue);
            string xLabel = Compiler.GetFullName(aLabel, true);
            Asm.Emit(xJumpOpCode, xLabel);
        }

        [Emitter(typeof(If), typeof(Reg), typeof(OpCompare), typeof(Variable), typeof(GotoKeyword), typeof(Identifier))]
        protected void IfRegisterConditionVariable(string aOpIf, Register aRegister, string aOpCompare, Address aValue, string aGotoKeyword, string aLabel)
        {
            var xJumpOpCode = GetJumpOpCode(aOpCompare);
            aValue.AddPrefix(Compiler.CurrentNamespace);
            Asm.Emit(OpCode.Cmp, aRegister, aValue);
            string xLabel = Compiler.GetFullName(aLabel, true);
            Asm.Emit(xJumpOpCode, xLabel);
        }

        [Emitter(typeof(If), typeof(Reg), typeof(OpCompare), typeof(VariableAddress), typeof(GotoKeyword), typeof(Identifier))]
        protected void IfRegisterConditionVariableAddressGoto(string aOpIf, Register aRegister, string aOpCompare, string aValue, string aGotoKeyword, string aLabel)
        {
            var xJumpOpCode = GetJumpOpCode(aOpCompare);
            string xValue = Compiler.GetFullName(aValue);
            Asm.Emit(OpCode.Cmp, aRegister, xValue);
            string xLabel = Compiler.GetFullName(aLabel, true);
            Asm.Emit(xJumpOpCode, xLabel);
        }
        #endregion

        // if AL = #Vs2Ds_Noop {
        [Emitter(typeof(If), typeof(Size), typeof(CompareVar), typeof(OpOpenBrace))]
        protected void IfConditionBlockStart(string aOpIf, string aSize, object[] aCompareData, object aOpOpenBrace)
        {
            Compiler.Blocks.StartBlock(Compiler.BlockType.If);

            var xJumpOpCode = GetOppositeJumpOpCode(aCompareData[1].ToString());
            aCompareData[0] = GetFullNameOrAddPrefix(aCompareData[0]);
            aCompareData[2] = GetFullNameOrAddPrefix(aCompareData[2]);

            Asm.Emit(OpCode.Cmp, aSize, aCompareData[0], aCompareData[2]);
            Asm.Emit(xJumpOpCode, Compiler.Blocks.EndBlockLabel);
        }

        // if AL = goto lLabel123
        [Emitter(typeof(If), typeof(Size), typeof(CompareVar), typeof(GotoKeyword), typeof(Identifier))]
        protected void IfConditionGoto(string aOpIf, string aSize, object[] aCompareData, string aGotoKeyword, string aLabel)
        {
            aCompareData[0] = GetFullNameOrAddPrefix(aCompareData[0]);
            aCompareData[2] = GetFullNameOrAddPrefix(aCompareData[2]);
        }

        // If = return
        [Emitter(typeof(If), typeof(OpPureComparators), typeof(Return))]
        protected void IfConditionPureReturn(string aOpIf, string aPureComparator, string aReturns)
        {
        }

        [Emitter(typeof(If), typeof(OpPureComparators), typeof(OpOpenBrace))]
        protected void IfConditionPureBlockStart(string aOpIf, string aOpPureComparators, string aOpOpenBrace)
        {
            Compiler.Blocks.StartBlock(Compiler.BlockType.If);
        }

        [Emitter(typeof(If), typeof(OpPureComparators), typeof(GotoKeyword), typeof(Identifier))]
        protected void IfConditionPureGoto(string aOpIf, string aOpPureComparators, string aGotoKeyword, string aLabel)
        {
        }

        private static OpCode GetJumpOpCode(string aCompare)
        {
            switch (aCompare)
            {
                case "<":
                    return OpCode.Jl;
                case "<=":
                    return OpCode.Jle;
                case ">":
                    return OpCode.Jg;
                case ">=":
                    return OpCode.Jge;
                case "=":
                    return OpCode.Je;
                case "!=":
                    return OpCode.Jne;
                default:
                    throw new Exception($"Unknown comparison opcode '{aCompare}'");
            }
        }

        private static OpCode GetOppositeJumpOpCode(string aCompare)
        {
            switch (aCompare)
            {
                case "<":
                    return OpCode.Jge;
                case "<=":
                    return OpCode.Jg;
                case ">":
                    return OpCode.Jle;
                case ">=":
                    return OpCode.Jl;
                case "=":
                    return OpCode.Jne;
                case "!=":
                    return OpCode.Je;
                default:
                    throw new Exception($"Unknown comparison opcode '{aCompare}'");
            }
        }

        private object GetFullNameOrAddPrefix(object aOperand)
        {
            if (aOperand is x86.Params.Address xAddress)
            {
                xAddress.AddPrefix(Compiler.CurrentNamespace);
                return xAddress;
            }
            else if (aOperand is string s)
            {
                aOperand = Compiler.GetFullName(s);
            }
            return aOperand;
        }
    }
}
