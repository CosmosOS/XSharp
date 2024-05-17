namespace XSharp.Assembler.x86.SSE
{
	[XSharp.Assembler.OpCode("cvtsi2sd")]
	public class ConvertSI2SD : InstructionWithDestinationAndSource
	{
        public ConvertSI2SD()
        {
            SourceRequiresSize = true;
        }
	}
}
