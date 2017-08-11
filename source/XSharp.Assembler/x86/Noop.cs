namespace XSharp.Assembler.x86
{
    [XSharp.Assembler.OpCode("NOP")]
    public class Noop : Instruction
    {
    }

    [XSharp.Assembler.OpCode("NOP ; INT3")]
    public class DebugNoop : Instruction
    {
    }
}