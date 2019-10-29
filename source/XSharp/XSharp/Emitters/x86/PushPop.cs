using Spruce.Attribs;
using Spruce.Tokens;
using XSharp.Tokens;
using XSharp.x86;
using XSharp.x86.Params;
using Reg = XSharp.Tokens.Reg;

namespace XSharp.x86.Emitters
{
    /// <summary>
    /// Push and Pop values
    /// </summary>
    /// <seealso cref="XSharp.x86.Emitters.Emitters" />
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
            Asm.Emit(OpCode.Push, $"{Compiler.CurrentNamespace}_Const_{value}");
        }

        [Emitter(typeof(OpPlus), typeof(Variable))]
        protected void PushVar(string aOpPlus, Address value)
        {
            // TODO: Do this better? Use Compiler.GetFullName() so things are consistent.
            value.AddPrefix(Compiler.CurrentNamespace);
            Asm.Emit(OpCode.Push, value);
        }

        [Emitter(typeof(OpPlus), typeof(VariableAddress))]
        protected void PushVarAddr(string aOpPlus, string value)
        {
            Asm.Emit(OpCode.Push, $"{Compiler.CurrentNamespace}_{value}");
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
            Asm.Emit(OpCode.Pop, $"{Compiler.CurrentNamespace}_Const_{value}");
        }

        [Emitter(typeof(OpMinus), typeof(Variable))]
        protected void PopVar(string aOpPlus, Address value)
        {
            // TODO: Do this better? Use Compiler.GetFullName() so things are consistent.
            value.AddPrefix(Compiler.CurrentNamespace);
            Asm.Emit(OpCode.Pop, value);
        }

        [Emitter(typeof(OpMinus), typeof(VariableAddress))]
        protected void PopAddr(string aOpPlus, string value)
        {
            Asm.Emit(OpCode.Pop, $"{Compiler.CurrentNamespace}_{value}");
        }
    }
}
