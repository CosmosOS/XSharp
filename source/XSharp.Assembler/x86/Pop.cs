namespace XSharp.Assembler.x86
{
    [XSharp.Assembler.OpCode("pop")]
	public class Pop: InstructionWithDestinationAndSize{
        public Pop() : base("pop")
        {
        }
	}

}