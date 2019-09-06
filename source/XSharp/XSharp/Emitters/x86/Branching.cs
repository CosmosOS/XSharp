using System;
using System.Linq;
using Spruce.Attribs;
using Spruce.Tokens;
using XSharp.Tokens;

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

        // if AL = #Vs2Ds_Noop return
        [Emitter(typeof(If), typeof(Compare), typeof(Return))]
        protected void IfConditionReturn(string aOpIf, object[] aCompareData, object aOpReturn)
        {
            var xJumpOpCode = GetJumpOpCode(aCompareData[1].ToString());
            aCompareData[0] = GetFullNameOrAddPrefix(aCompareData[0]);
            aCompareData[2] = GetFullNameOrAddPrefix(aCompareData[2]);

            Asm.Emit(OpCode.Cmp, aCompareData[0], aCompareData[2]);
            Asm.Emit(xJumpOpCode, Compiler.CurrentFunctionExitLabel);
        }

        // if = {
        [Emitter(typeof(If), typeof(Compare), typeof(OpOpenBrace))]
        protected void IfConditionBlockStart(string aOpIf, object[] aCompareData, object aOpOpenBrace)
        {
            Compiler.Blocks.StartBlock(Compiler.BlockType.If);

            var xJumpOpCode = GetOppositeJumpOpCode(aCompareData[1].ToString());
            aCompareData[0] = GetFullNameOrAddPrefix(aCompareData[0]);
            aCompareData[2] = GetFullNameOrAddPrefix(aCompareData[2]);

            Asm.Emit(OpCode.Cmp, aCompareData[0], aCompareData[2]);
            Asm.Emit(xJumpOpCode, Compiler.Blocks.EndBlockLabel);
        }

        // if = goto lLabel123
        [Emitter(typeof(If), typeof(Compare), typeof(GotoKeyword), typeof(Identifier))]
        protected void IfConditionGoto(string aOpIf, object[] aCompareData, string aGotoKeyword, string aLabel)
        {
            aCompareData[0] = GetFullNameOrAddPrefix(aCompareData[0]);
            aCompareData[2] = GetFullNameOrAddPrefix(aCompareData[2]);
        }

        // if AL = #Vs2Ds_Noop return
        [Emitter(typeof(If), typeof(Size), typeof(CompareVar), typeof(Return))]
        protected void IfConditionReturn(string aOpIf, string aSize, object[] aCompareData, object aOpReturn)
        {
            aCompareData[0] = GetFullNameOrAddPrefix(aCompareData[0]);
            aCompareData[2] = GetFullNameOrAddPrefix(aCompareData[2]);
        }

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
