namespace XSharp.Assembler.x86 {
    [XSharp.Assembler.OpCode("int")]
    public class INT : InstructionWithDestination {
        public override void WriteText( XSharp.Assembler.Assembler aAssembler, System.IO.TextWriter aOutput )
        {
          //TODO: In base have a property that has the opcode from above and we can reuse it.
            aOutput.Write("Int " + DestinationValue);
        }
    }
}