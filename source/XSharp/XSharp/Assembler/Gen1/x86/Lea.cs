namespace XSharp.Assembler.x86
{
    /// <summary>
    /// Represents the LEA-instruction (load effective address, 0x8d)
    /// </summary>
    [XSharp.Assembler.OpCode("lea")]
    public class Lea
        : InstructionWithDestinationAndSourceAndSize
    {
        public Lea() : base("lea")
        {
        }
    }
}
