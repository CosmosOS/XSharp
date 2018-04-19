namespace XSharp.Assembler.x86
{
    [XSharp.Assembler.OpCode("cmp")]
	public class Compare: InstructionWithDestinationAndSourceAndSize {
        public Compare() : base("cmp")
        {
        }
	}
}