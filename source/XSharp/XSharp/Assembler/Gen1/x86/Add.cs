namespace XSharp.Assembler.x86
{
    [XSharp.Assembler.OpCode("add")]
	public class Add: InstructionWithDestinationAndSourceAndSize {
        public Add() : base("add")
        {
        }
	}
}