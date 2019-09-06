using Spruce.Attribs;
using XSharp.Tokens;
using XSharp.x86;

namespace XSharp.x86.Emitters
{
    /// <summary>
    /// TEST: Logical compare
    /// </summary>
    /// <seealso cref="XSharp.x86.Emitters.Emitters" />
    public class Test : Emitters
    {
        public Test(Compiler aCompiler, x86.Assemblers.Assembler aAsm) : base(aCompiler, aAsm)
        {
        }

        [Emitter(typeof(Reg08), typeof(TestKeyword), typeof(Int08u))]
        [Emitter(typeof(Reg16), typeof(TestKeyword), typeof(Int16u))]
        [Emitter(typeof(Reg32), typeof(TestKeyword), typeof(Int32u))]
        protected void TestRegWithInt(Register aRegister, string aTestKeyword, object aValue)
        {
            Asm.Emit(OpCode.Test, aRegister, aValue);
        }
    }
}
