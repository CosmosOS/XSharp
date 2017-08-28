using Spruce.Attribs;
using Spruce.Tokens;
using XSharp.Tokens;
using XSharp.x86;

namespace XSharp
{
    // Emitters does the actual translation from X# (via Spruce) to x86 (via Assemblers)
    public class Emitters
    {
        public readonly Compiler Compiler;
        public readonly x86.Assemblers.Assembler Asm;

        public Emitters(Compiler aCompiler, x86.Assemblers.Assembler aAsm)
        {
            Compiler = aCompiler;
            Asm = aAsm;
        }

        // Temp TEST
        [Emitter(typeof(If), typeof(Compare32))]
        protected void Test(string aIf, object[] aData) {
            Compiler.WriteLine("Woohoo!");
        }

        [Emitter(typeof(OpLiteral), typeof(All))] // //! Literal NASM Output
        protected void Literal(string aOp, string aText) {
            Compiler.WriteLine(aText);
        }

        [Emitter(typeof(OpComment), typeof(All))] // // Comment text
        protected void Comment(string aOp, string aText)
        {
            if (Compiler.EmitUserComments)
            {
                Compiler.WriteLine("; " + aText);
            }
        }

        [Emitter(typeof(Namespace), typeof(AlphaNum))] // namespace name
        protected void Namespace(string aNamespace, string aText)
        {
        }

        // MUST be before RegXX,OpMath,... because of + vs ++
        [Emitter(typeof(RegXX), typeof(OpIncDec))]
        protected void IncrementDecrement(string aRegister, object aOpIncrementDecrement)
        {
        }

        // Don't use RegXX. This method ensures proper data sizes.
        [Emitter(typeof(Reg08), typeof(OpEquals), typeof(Int08u))] // AH = 0
        [Emitter(typeof(Reg16), typeof(OpEquals), typeof(Int16u))] // AX = 0
        [Emitter(typeof(Reg32), typeof(OpEquals), typeof(Int32u))] // EAX = 0
        [Emitter(typeof(RegXX), typeof(OpEquals), typeof(Variable))] // EAX = 0
        [Emitter(typeof(RegXX), typeof(OpEquals), typeof(Constant))] // EAX = 0
        [Emitter(typeof(Reg32), typeof(OpEquals), typeof(VariableAddress))] // EAX = 0
        protected void RegAssignNum(string aReg, string aEquals, object aVal)
        {
            Asm.Emit(OpCode.Mov, aReg, aVal);
        }

        [Emitter(typeof(Variable), typeof(OpEquals), typeof(Int08u))]
        [Emitter(typeof(Variable), typeof(OpEquals), typeof(Int16u))]
        [Emitter(typeof(Variable), typeof(OpEquals), typeof(Int32u))]
        [Emitter(typeof(Variable), typeof(OpEquals), typeof(Variable))]
        [Emitter(typeof(Variable), typeof(OpEquals), typeof(VariableAddress))]
        [Emitter(typeof(Variable), typeof(OpEquals), typeof(Constant))]
        [Emitter(typeof(Variable), typeof(OpEquals), typeof(RegXX))]
        protected void VariableAssignment(string aVariableName, string aOpEquals, object aValue)
        {
        }

        [Emitter(typeof(NOP))]
        [Emitter(typeof(Return))]
        [Emitter(typeof(PushAll))]
        [Emitter(typeof(PopAll))]
        protected void ZeroParamOp(OpCode aOpCode)
        {
            Asm.Emit(aOpCode);
        }

        // +RegXX
        [Emitter(typeof(OpPlus), typeof(RegXX))]
        [Emitter(typeof(OpPlus), typeof(Constant))]
        [Emitter(typeof(OpPlus), typeof(Variable))]
        [Emitter(typeof(OpPlus), typeof(VariableAddress))]
        [Emitter(typeof(OpPlus), typeof(Int08u))]
        [Emitter(typeof(OpPlus), typeof(Int16u))]
        [Emitter(typeof(OpPlus), typeof(Int32u))]
        protected void RegPush(string aOp, object aReg)
        {
        }

        // -RegXX
        [Emitter(typeof(OpMinus), typeof(RegXX))]
        [Emitter(typeof(OpMinus), typeof(Constant))]
        [Emitter(typeof(OpMinus), typeof(Variable))]
        [Emitter(typeof(OpMinus), typeof(VariableAddress))]
        [Emitter(typeof(OpMinus), typeof(Int08u))]
        [Emitter(typeof(OpMinus), typeof(Int16u))]
        [Emitter(typeof(OpMinus), typeof(Int32u))]
        protected void RegPop(string aOp, object aReg)
        {
        }

        // if AL = #Vs2Ds_Noop return
        [Emitter(typeof(If), typeof(RegXX), typeof(OpCompare), typeof(Constant), typeof(Return))]
        [Emitter(typeof(If), typeof(RegXX), typeof(OpCompare), typeof(Variable), typeof(Return))]
        [Emitter(typeof(If), typeof(Reg32), typeof(OpCompare), typeof(VariableAddress), typeof(Return))]
        [Emitter(typeof(If), typeof(Reg08), typeof(OpCompare), typeof(Int08u), typeof(Return))]
        [Emitter(typeof(If), typeof(Reg16), typeof(OpCompare), typeof(Int16u), typeof(Return))]
        [Emitter(typeof(If), typeof(Reg32), typeof(OpCompare), typeof(Int32u), typeof(Return))]
        [Emitter(typeof(If), typeof(Constant), typeof(OpCompare), typeof(Constant), typeof(Return))]
        [Emitter(typeof(If), typeof(Constant), typeof(OpCompare), typeof(Variable), typeof(Return))]
        [Emitter(typeof(If), typeof(Constant), typeof(OpCompare), typeof(VariableAddress), typeof(Return))]
        [Emitter(typeof(If), typeof(Constant), typeof(OpCompare), typeof(Int08u), typeof(Return))]
        [Emitter(typeof(If), typeof(Constant), typeof(OpCompare), typeof(Int16u), typeof(Return))]
        [Emitter(typeof(If), typeof(Constant), typeof(OpCompare), typeof(Int32u), typeof(Return))]
        [Emitter(typeof(If), typeof(Variable), typeof(OpCompare), typeof(Constant), typeof(Return))]
        [Emitter(typeof(If), typeof(Variable), typeof(OpCompare), typeof(Variable), typeof(Return))]
        [Emitter(typeof(If), typeof(Variable), typeof(OpCompare), typeof(VariableAddress), typeof(Return))]
        [Emitter(typeof(If), typeof(Variable), typeof(OpCompare), typeof(Int08u), typeof(Return))]
        [Emitter(typeof(If), typeof(Variable), typeof(OpCompare), typeof(Int16u), typeof(Return))]
        [Emitter(typeof(If), typeof(Variable), typeof(OpCompare), typeof(Int32u), typeof(Return))]
        [Emitter(typeof(If), typeof(VariableAddress), typeof(OpCompare), typeof(Constant), typeof(Return))]
        [Emitter(typeof(If), typeof(VariableAddress), typeof(OpCompare), typeof(Variable), typeof(Return))]
        [Emitter(typeof(If), typeof(VariableAddress), typeof(OpCompare), typeof(VariableAddress), typeof(Return))]
        [Emitter(typeof(If), typeof(VariableAddress), typeof(OpCompare), typeof(Int32u), typeof(Return))]
        [Emitter(typeof(If), typeof(Int08u), typeof(OpCompare), typeof(Int08u), typeof(Return))]
        [Emitter(typeof(If), typeof(Int08u), typeof(OpCompare), typeof(Constant), typeof(Return))]
        [Emitter(typeof(If), typeof(Int08u), typeof(OpCompare), typeof(Variable), typeof(Return))]
        [Emitter(typeof(If), typeof(Int16u), typeof(OpCompare), typeof(Int16u), typeof(Return))]
        [Emitter(typeof(If), typeof(Int16u), typeof(OpCompare), typeof(Constant), typeof(Return))]
        [Emitter(typeof(If), typeof(Int16u), typeof(OpCompare), typeof(Variable), typeof(Return))]
        [Emitter(typeof(If), typeof(Int32u), typeof(OpCompare), typeof(Int32u), typeof(Return))]
        [Emitter(typeof(If), typeof(Int32u), typeof(OpCompare), typeof(Constant), typeof(Return))]
        [Emitter(typeof(If), typeof(Int32u), typeof(OpCompare), typeof(Variable), typeof(Return))]
        [Emitter(typeof(If), typeof(Int32u), typeof(OpCompare), typeof(VariableAddress), typeof(Return))]
        protected void IfCondition(string aOpIf, object aLeftValue, string aOpEquals, object aRightValue,
            object aOpReturn)
        {
        }

        // If = return
        [Emitter(typeof(If), typeof(OpEquals), typeof(Return))]
        protected void IfConditionPure(string aOpIf, string aEquals, string aReturns)
        {
        }

        // const i = 0
        [Emitter(typeof(ConstKeyword), typeof(AlphaNum), typeof(OpEquals), typeof(Int08u))]
        [Emitter(typeof(ConstKeyword), typeof(AlphaNum), typeof(OpEquals), typeof(Int16u))]
        [Emitter(typeof(ConstKeyword), typeof(AlphaNum), typeof(OpEquals), typeof(Int32u))]
        [Emitter(typeof(ConstKeyword), typeof(AlphaNum), typeof(OpEquals), typeof(String))]
        protected void ConstDefinition(string aConstKeyword, string aConstName, string oOpEquals, object aConstValue)
        {
        }

        // const i = 0
        [Emitter(typeof(VarKeyword), typeof(AlphaNum), typeof(OpEquals), typeof(Int08u))]
        [Emitter(typeof(VarKeyword), typeof(AlphaNum), typeof(OpEquals), typeof(Int16u))]
        [Emitter(typeof(VarKeyword), typeof(AlphaNum), typeof(OpEquals), typeof(Int32u))]
        [Emitter(typeof(VarKeyword), typeof(AlphaNum), typeof(OpEquals), typeof(String))]
        [Emitter(typeof(VarKeyword), typeof(AlphaNum), typeof(OpEquals), typeof(Constant))]
        [Emitter(typeof(VarKeyword), typeof(AlphaNum), typeof(OpEquals), typeof(Variable))]
        [Emitter(typeof(VarKeyword), typeof(AlphaNum), typeof(OpEquals), typeof(VariableAddress))]
        protected void VariableDefinition(string aVarKeyword, string aVariableName, string oOpEquals, object aVariableValue)
        {
        }

        // EAX = #constVal
        [Emitter(typeof(RegXX), typeof(OpMath), typeof(Constant))]
        [Emitter(typeof(RegXX), typeof(OpMath), typeof(Variable))]
        [Emitter(typeof(Reg08), typeof(OpMath), typeof(Reg08))]
        [Emitter(typeof(Reg08), typeof(OpMath), typeof(Int08u))]
        [Emitter(typeof(Reg16), typeof(OpMath), typeof(Reg16))]
        [Emitter(typeof(Reg16), typeof(OpMath), typeof(Int16u))]
        [Emitter(typeof(Reg32), typeof(OpMath), typeof(Reg32))]
        [Emitter(typeof(Reg32), typeof(OpMath), typeof(Int32u))]
        [Emitter(typeof(Reg32), typeof(OpMath), typeof(VariableAddress))]
        protected void Arithmetic(string aRegister, string aOpArithmetic, object aValue)
        {
        }

        [Emitter(typeof(RegXX), typeof(OpBitwise), typeof(Constant))]
        [Emitter(typeof(RegXX), typeof(OpBitwise), typeof(Variable))]
        [Emitter(typeof(Reg08), typeof(OpBitwise), typeof(Reg08))]
        [Emitter(typeof(Reg08), typeof(OpBitwise), typeof(Int08u))]
        [Emitter(typeof(Reg16), typeof(OpBitwise), typeof(Reg16))]
        [Emitter(typeof(Reg16), typeof(OpBitwise), typeof(Int16u))]
        [Emitter(typeof(Reg32), typeof(OpBitwise), typeof(Reg32))]
        [Emitter(typeof(Reg32), typeof(OpBitwise), typeof(Int32u))]
        [Emitter(typeof(Reg32), typeof(OpBitwise), typeof(VariableAddress))]
        protected void BitwiseArithmetic(string aRegister, string aOpBitwise, object aValue)
        {
        }

        [Emitter(typeof(Reg08), typeof(OpEquals), typeof(OpTilde), typeof(Reg08))]
        [Emitter(typeof(Reg16), typeof(OpEquals), typeof(OpTilde), typeof(Reg16))]
        [Emitter(typeof(Reg32), typeof(OpEquals), typeof(OpTilde), typeof(Reg32))]
        protected void BitwiseNot(string aRegister, string aOpEquals, string aOpTilde, string aSourceRegister)
        {
        }

        [Emitter(typeof(RegXX), typeof(OpShift), typeof(Int08u))]
        [Emitter(typeof(RegXX), typeof(OpShift), typeof(Int16u))]
        [Emitter(typeof(RegXX), typeof(OpShift), typeof(Int32u))]
        protected void BitwiseShift(string aRegister, string aBitwiseShift, object aNumberBits)
        {
        }

        // function fName123 {
        [Emitter(typeof(Function), typeof(AlphaNum), typeof(OpOpenBrace))]
        protected void FunctionDefinitionStart(string funcKeyword, string functionName, string opOpenBraces)
        {
        }

        // }
        [Emitter(typeof(OpCloseBrace))]
        protected void FunctionDefinitionEnd(string opCloseBrace)
        {
        }

        // Important! All that start with AlphaNum MUST be last to allow fall through to prevent early claims over keywords.
        // fName ()
        [Emitter(typeof(AlphaNum), typeof(OpOpenParen), typeof(OpCloseParen))]
        protected void FunctionCall(string functionName, string opOpenParanthesis, string opCloseParanthesis)
        {
        }

        // Label
        [Emitter(typeof(AlphaNum), typeof(OpColon))]
        protected void LabelDefinition(string aLabelName, string aOpColon)
        {
        }
    }
}
