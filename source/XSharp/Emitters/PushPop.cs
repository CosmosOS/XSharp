using Spruce.Attribs;
using Spruce.Tokens;
using XSharp.Tokens;
using XSharp.x86;
using XSharp.x86.Params;
using Reg = XSharp.Tokens.Reg;

namespace XSharp.Emitters
{
    /// <summary>
    /// Push and Pop values
    /// </summary>
    /// <seealso cref="XSharp.Emitters.Emitters" />
    public class PushPop : Emitters
    {
        public PushPop(Compiler aCompiler, x86.Assemblers.Assembler aAsm) : base(aCompiler, aAsm)
        {
        }

        // +Reg
        [Emitter(typeof(OpPlus), typeof(Reg))]
        [Emitter(typeof(OpPlus), typeof(Int08u))]
        [Emitter(typeof(OpPlus), typeof(Int16u))]
        [Emitter(typeof(OpPlus), typeof(Int32u))]
        protected void PushValue(string aOp, object aReg)
        {
            Asm.Emit(OpCode.Push, aReg);
        }

        [Emitter(typeof(OpPlus), typeof(Const))]
        protected void PushConst(string aOpPlus, string value)
        {
            Asm.Emit(OpCode.Push, Compiler.GetPrefixForConst + value);
        }

        [Emitter(typeof(OpPlus), typeof(Variable))]
        protected void PushVar(string aOpPlus, object value)
        {
            Asm.Emit(OpCode.Push, Compiler.GetPrefixForVar + value);
        }

        [Emitter(typeof(OpPlus), typeof(VariableAddress))]
        protected void PushVarAddr(string aOpPlus, Address value)
        {
            value.AddressOf = $"{Compiler.GetPrefixForVar}{value.AddressOf}";
            Asm.Emit(OpCode.Push, value);
        }

        // -Reg
        [Emitter(typeof(OpMinus), typeof(Reg))]
        [Emitter(typeof(OpMinus), typeof(Int08u))]
        [Emitter(typeof(OpMinus), typeof(Int16u))]
        [Emitter(typeof(OpMinus), typeof(Int32u))]
        protected void PopValue(string aOp, object aReg)
        {
            Asm.Emit(OpCode.Pop, aReg);
        }

        [Emitter(typeof(OpMinus), typeof(Const))]
        protected void PopConst(string aOpMinus, string value)
        {
            Asm.Emit(OpCode.Pop, Compiler.GetPrefixForConst + value);
        }

        [Emitter(typeof(OpMinus), typeof(Variable))]
        protected void PopVar(string aOpMinus, object value)
        {
            Asm.Emit(OpCode.Pop, Compiler.GetPrefixForVar + value);
        }

        [Emitter(typeof(OpMinus), typeof(VariableAddress))]
        protected void PopVarAddr(string aOpMinus, Address value)
        {
            value.AddressOf = $"{Compiler.GetPrefixForVar}{value.AddressOf}";
            Asm.Emit(OpCode.Pop, value);
        }
    }
}
