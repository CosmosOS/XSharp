namespace XSharp.Assembler.x86
{
    /// <summary>
    /// Represents the RDMSR-instruction (read model specific register, 0x0f 0x32)
    /// </summary>
    [OpCode("rdmsr")]
    public class Rdmsr : Instruction
    {
        public Rdmsr() : base("rdmsr")
        {
        }
    }
}
