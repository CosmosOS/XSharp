using System;
using System.Text;
using Spruce.Attribs;
using Spruce.Tokens;
using XSharp.Tokens;
using System.Linq;
using XSharp.x86;

namespace XSharp.x86.Emitters
{
    // Emitters does the actual translation from X# (via Spruce) to x86 (via Assemblers)
    public class AllEmitters : Emitters
    {
        public AllEmitters(Compiler aCompiler, x86.Assemblers.Assembler aAsm) : base(aCompiler, aAsm)
        {
        }

        // ===============================================================
        // Keywords

        // ===============================================================

        [Emitter(typeof(OpInclude), typeof(All))]
        protected void IncludeStatement(object aOp, string aText)
        {
            var possibleIncludes = Compiler.SourceProviders.Select(provider => provider.Invoke(aText)).Where(toInclude => toInclude != null).ToList();
            if(possibleIncludes.Count == 0)
            {
                throw new Exception($"Invalid include statement: No source provider found for {aText}");
            }
            else if(possibleIncludes.Count != 1)
            {
                throw new Exception($"Invalid include statement: Multiple source providers found for {aText}");
            }
            else
            {
                Compiler.Emit(possibleIncludes[0]);
            }
        }

        [Emitter(typeof(Variable), typeof(OpEquals), typeof(Variable))]
        protected void VariableAssignment(object aVariableName, string aOpEquals, object aValue)
        {
            throw new NotImplementedException();
        }

        [Emitter(typeof(While), typeof(Compare), typeof(OpOpenBrace))]
        protected void WhileConditionBlockStart(string aOpWhile, object[] aCompareData, object aOpOpenBrace)
        {
            Compiler.Blocks.StartBlock(Compiler.BlockType.While);
        }

        [Emitter(typeof(While), typeof(Size), typeof(CompareWithMem), typeof(OpOpenBrace))]
        protected void WhileConditionWithMemoryBlockStart(string aOpWhile, string aSize, object[] aCompareData, object aOpOpenBrace)
        {
            Compiler.Blocks.StartBlock(Compiler.BlockType.While);
        }

        [Emitter(typeof(While), typeof(OpPureComparators), typeof(OpOpenBrace))]
        protected void WhileConditionPureBlockStart(string aOpWhile, string aOpPureComparators, string aOpOpenBrace)
        {
            Compiler.Blocks.StartBlock(Compiler.BlockType.If);
        }

        [Emitter(typeof(Repeat), typeof(Int32u), typeof(Times), typeof(OpOpenBrace))]
        protected void RepeatBlockStart(string aOpRepeat, UInt32 loops, string aOpTimes, string aOpOpenBrace)
        {
            Compiler.Blocks.StartBlock(Compiler.BlockType.Repeat);
        }

        // const i = 0
        [Emitter(typeof(ConstKeyword), typeof(Identifier), typeof(OpEquals), typeof(Int32u))]
        protected void ConstDefinition(string aConstKeyword, string aConstName, string oOpEquals, object aConstValue)
        {
            string xConstName = Compiler.GetFullName($"Const_{aConstName}");
            Compiler.WriteLine($"{xConstName} equ {aConstValue}");
        }

        [Emitter(typeof(ConstKeyword), typeof(Identifier), typeof(OpEquals), typeof(StringLiteral))]
        protected void ConstDefinition(string aConstKeyword, string aConstName, string oOpEquals, string aConstValue)
        {
            string xConstName = Compiler.GetFullName($"Const_{aConstName}");
            byte[] chars = Encoding.ASCII.GetBytes(aConstValue);
            Compiler.WriteLine($"{xConstName}:");
            Compiler.WriteLine($"\t\tdb {string.Join(", ", chars)}");
        }

        [Emitter(typeof(VarKeyword), typeof(Identifier), typeof(OpEquals), typeof(Const))]
        [Emitter(typeof(VarKeyword), typeof(Identifier), typeof(OpEquals), typeof(Variable))]
        [Emitter(typeof(VarKeyword), typeof(Identifier), typeof(OpEquals), typeof(VariableAddress))]
        protected void VariableDefinition(string aVarKeyword, string aVariableName, string oOpEquals, object aVariableValue)
        {
            throw new NotImplementedException();
        }

        [Emitter(typeof(VarKeyword), typeof(Identifier), typeof(OpEquals), typeof(Int32u))]
        protected void VariableDefinition(string aVarKeyword, string aVariableName, string oOpEquals, uint aVariableValue)
        {
            string xVariableName = Compiler.GetFullName(aVariableName);
            Compiler.WriteLine($"{xVariableName} dd {aVariableValue}");
        }

        [Emitter(typeof(VarKeyword), typeof(Identifier), typeof(OpEquals), typeof(StringLiteral))]
        protected void VariableDefinition(string aVarKeyword, string aVariableName, string oOpEquals, string aStringLiteral)
        {
            string xVariableName = Compiler.GetFullName(aVariableName);
            byte[] chars = Encoding.ASCII.GetBytes(aStringLiteral);
            Compiler.WriteLine($"{xVariableName}:");
            Compiler.WriteLine($"\t\tdb {string.Join(", ", chars)}");
        }

        [Emitter(typeof(VarKeyword), typeof(Identifier))]
        protected void VariableDefinition(string aVarKeyword, string aVariableName)
        {
            string xVariableName = Compiler.GetFullName(aVariableName);
            Compiler.WriteLine($"{xVariableName} dd 0");
        }

        [Emitter(typeof(VarKeyword), typeof(Identifier), typeof(Size), typeof(OpOpenBracket), typeof(Int32u), typeof(OpCloseBracket))]
        protected void VariableArrayDefinition(string aVarKeyword, string aVariableName, string aSize, string aOpOpenBracket, object aNumberOfItems, string aOpCloseBracket)
        {
            string xVariableName = Compiler.GetFullName(aVariableName);
            Compiler.WriteLine($"{xVariableName} dd 0");
        }

        // interrupt iNmae123 {
        [Emitter(typeof(Interrupt), typeof(Identifier), typeof(OpOpenBrace))]
        protected void InterruptDefinitionStart(string aInterruptKeyword, string aFunctionName, string aOpOpenBrace)
        {
            if (!string.IsNullOrWhiteSpace(Compiler.CurrentFunction))
            {
                throw new Exception("Found an interrupt handler embedded inside another interrupt handler or function.");
            }

            Compiler.CurrentFunction = aFunctionName;
            Compiler.CurrentFunctionType = Compiler.BlockType.Interrupt;
            Compiler.FunctionExitLabelFound = false;

            Compiler.Blocks.Reset();

            string xFunctionName = Compiler.GetFullName(aFunctionName);
            Compiler.WriteLine($"{xFunctionName}:");
        }

        // function fName123 {
        [Emitter(typeof(FunctionKeyword), typeof(Identifier), typeof(OpOpenBrace))]
        protected void FunctionDefinitionStart(string aFunctionKeyword, string aFunctionName, string aOpOpenBrace)
        {
            if (!string.IsNullOrWhiteSpace(Compiler.CurrentFunction))
            {
                throw new Exception("Found a function definition embedded inside another interrupt handler or function.");
            }

            Compiler.CurrentFunction = aFunctionName;
            Compiler.CurrentFunctionType = Compiler.BlockType.Function;
            Compiler.FunctionExitLabelFound = false;

            Compiler.Blocks.Reset();

            string xFunctionName = Compiler.GetFullName(aFunctionName);
            Compiler.WriteLine($"{xFunctionName}:");
        }

        // }
        [Emitter(typeof(OpCloseBrace))]
        protected void BlockEnd(string opCloseBrace)
        {
            var xBlock = Compiler.Blocks.Current();
            if (xBlock != null)
            {
                switch (xBlock.Type)
                {
                    case Compiler.BlockType.If:
                    case Compiler.BlockType.While:
                    case Compiler.BlockType.Repeat:
                        Compiler.Blocks.EndBlock();
                        break;
                }
            }
            else
            {
                // No current block. Must be a function or interrupt.
                if (!Compiler.FunctionExitLabelFound)
                {
                    // Need to emit an 'Exit:' label.
                    Compiler.WriteLine($"{Compiler.CurrentFunctionExitLabel}:");
                }
                Asm.Emit(OpCode.Mov, "dword", new x86.Params.Address("INTS_LastKnownAddress"), Compiler.CurrentFunctionExitLabel);

                switch (Compiler.CurrentFunctionType)
                {
                    case Compiler.BlockType.Function:
                        Asm.Emit(OpCode.Ret);
                        break;
                    case Compiler.BlockType.Interrupt:
                        Asm.Emit(OpCode.IRet);
                        break;
                }

                Compiler.CurrentFunction = "";
                Compiler.CurrentFunctionType = Compiler.BlockType.None;
                Compiler.Blocks.Reset();
            }
        }

        [Emitter(typeof(GotoKeyword), typeof(Identifier))]
        protected void Goto(string aGotoKeyword, string aLabelName)
        {
            string xLabelName = Compiler.GetFullName(aLabelName, true);
            Asm.Emit(OpCode.Jmp, $"{xLabelName}");
        }

        // Important! All that start with AlphaNum MUST be last to allow fall through to prevent early claims over keywords.
        // fName ()
        [Emitter(typeof(Identifier), typeof(OpOpenParen), typeof(OpCloseParen))]
        protected void FunctionCall(string aFunctionName, string aOpOpenParenthesis, string aOpCloseParenthesis)
        {
            string xFunctionName = Compiler.GetFullName(aFunctionName);
            Compiler.WriteLine($"Call {xFunctionName}");
        }

        // lName123:
        [Emitter(typeof(Identifier), typeof(OpColon))]
        protected void LabelDefinitionStart(string aLabelName, string aOpColon)
        {
            Compiler.CurrentLabel = aLabelName;

            if (aLabelName.ToUpper() == "EXIT")
            {
                Compiler.FunctionExitLabelFound = true;
            }

            string xLabelName = Compiler.GetFullName(aLabelName, true);
            Compiler.WriteLine($"{xLabelName}:");
        }
    }
}
