namespace XSharp.Assembler.x86
{
    /// <summary>
    /// Represents the RDTSC-instruction (read timestamp counter, 0x0f 0x31)
    /// </summary>
    [OpCode("rdtsc")]
    public class Rdtsc : Instruction
    {
        public Rdtsc() : base("rdtsc")
        {
        }
    }
}
