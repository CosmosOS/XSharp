namespace XSharp.Assembler.x86
{
    [XSharp.Assembler.OpCode("push")]
    public class Push : InstructionWithDestinationAndSize {

        public Push():base("push") {
            Size = 32;
        }
    }
}
