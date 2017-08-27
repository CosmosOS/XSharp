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

        [Emitter(typeof(OpLiteral), typeof(All))] // //! Literal NASM Output
        protected void Literal(string aOp, string aText)
        {
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

        // Don't use RegXX. This method ensures proper data sizes.
        [Emitter(typeof(Reg08), typeof(OpEquals), typeof(Int08u))] // AH = 0
        [Emitter(typeof(Reg16), typeof(OpEquals), typeof(Int16u))] // AX = 0
        [Emitter(typeof(Reg32), typeof(OpEquals), typeof(Int32u))] // EAX = 0
        protected void RegAssignNum(string aReg, string aEquals, object aVal)
        {
            Asm.Emit(OpCode.Mov, aReg, aVal);
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
        [Emitter(typeof(If), typeof(RegXX), typeof(OpComparision), typeof(Constant), typeof(Return))]
        [Emitter(typeof(If), typeof(RegXX), typeof(OpComparision), typeof(Variable), typeof(Return))]
        [Emitter(typeof(If), typeof(Reg32), typeof(OpComparision), typeof(VariableAddress), typeof(Return))]
        [Emitter(typeof(If), typeof(Reg08), typeof(OpComparision), typeof(Int08u), typeof(Return))]
        [Emitter(typeof(If), typeof(Reg16), typeof(OpComparision), typeof(Int16u), typeof(Return))]
        [Emitter(typeof(If), typeof(Reg32), typeof(OpComparision), typeof(Int32u), typeof(Return))]
        [Emitter(typeof(If), typeof(Constant), typeof(OpComparision), typeof(Constant), typeof(Return))]
        [Emitter(typeof(If), typeof(Constant), typeof(OpComparision), typeof(Variable), typeof(Return))]
        [Emitter(typeof(If), typeof(Constant), typeof(OpComparision), typeof(VariableAddress), typeof(Return))]
        [Emitter(typeof(If), typeof(Constant), typeof(OpComparision), typeof(Int08u), typeof(Return))]
        [Emitter(typeof(If), typeof(Constant), typeof(OpComparision), typeof(Int16u), typeof(Return))]
        [Emitter(typeof(If), typeof(Constant), typeof(OpComparision), typeof(Int32u), typeof(Return))]
        [Emitter(typeof(If), typeof(Variable), typeof(OpComparision), typeof(Constant), typeof(Return))]
        [Emitter(typeof(If), typeof(Variable), typeof(OpComparision), typeof(Variable), typeof(Return))]
        [Emitter(typeof(If), typeof(Variable), typeof(OpComparision), typeof(VariableAddress), typeof(Return))]
        [Emitter(typeof(If), typeof(Variable), typeof(OpComparision), typeof(Int08u), typeof(Return))]
        [Emitter(typeof(If), typeof(Variable), typeof(OpComparision), typeof(Int16u), typeof(Return))]
        [Emitter(typeof(If), typeof(Variable), typeof(OpComparision), typeof(Int32u), typeof(Return))]
        [Emitter(typeof(If), typeof(VariableAddress), typeof(OpComparision), typeof(Constant), typeof(Return))]
        [Emitter(typeof(If), typeof(VariableAddress), typeof(OpComparision), typeof(Variable), typeof(Return))]
        [Emitter(typeof(If), typeof(VariableAddress), typeof(OpComparision), typeof(VariableAddress), typeof(Return))]
        [Emitter(typeof(If), typeof(VariableAddress), typeof(OpComparision), typeof(Int32u), typeof(Return))]
        [Emitter(typeof(If), typeof(Int08u), typeof(OpComparision), typeof(Int08u), typeof(Return))]
        [Emitter(typeof(If), typeof(Int08u), typeof(OpComparision), typeof(Constant), typeof(Return))]
        [Emitter(typeof(If), typeof(Int08u), typeof(OpComparision), typeof(Variable), typeof(Return))]
        [Emitter(typeof(If), typeof(Int16u), typeof(OpComparision), typeof(Int16u), typeof(Return))]
        [Emitter(typeof(If), typeof(Int16u), typeof(OpComparision), typeof(Constant), typeof(Return))]
        [Emitter(typeof(If), typeof(Int16u), typeof(OpComparision), typeof(Variable), typeof(Return))]
        [Emitter(typeof(If), typeof(Int32u), typeof(OpComparision), typeof(Int32u), typeof(Return))]
        [Emitter(typeof(If), typeof(Int32u), typeof(OpComparision), typeof(Constant), typeof(Return))]
        [Emitter(typeof(If), typeof(Int32u), typeof(OpComparision), typeof(Variable), typeof(Return))]
        [Emitter(typeof(If), typeof(Int32u), typeof(OpComparision), typeof(VariableAddress), typeof(Return))]
        protected void IfCondition(string aOpIf, object aLeftValue, string aOpEquals, object aRightValue,
            string aOpReturn)
        {
        }

        // If = return
        [Emitter(typeof(If), typeof(OpEquals), typeof(Return))]
        protected void IfConditionPure(string aOpIf, string aEquals, string aReturns)
        {
        }

        // const i = 0
        [Emitter(typeof(Const), typeof(AlphaNum), typeof(OpEquals), typeof(Int08u))]
        [Emitter(typeof(Const), typeof(AlphaNum), typeof(OpEquals), typeof(Int16u))]
        [Emitter(typeof(Const), typeof(AlphaNum), typeof(OpEquals), typeof(Int32u))]
        [Emitter(typeof(Const), typeof(AlphaNum), typeof(OpEquals), typeof(String))]
        protected void ConstDefinition(string aConstKeyword, string aConstName, string oOpEquals, object aConstValue)
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

        // Important! Last as fall through to prevent early claims over keywords.
        // fName ()
        [Emitter(typeof(AlphaNum), typeof(OpOpenParenthesis), typeof(OpCloseParenthesis))]
        protected void FunctionCall(string functionName, string opOpenParanthesis, string opCloseParanthesis)
        {
        }
    }
}
