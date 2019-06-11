namespace XSharp.Assembler.x86
{
    [XSharp.Assembler.OpCode("cmovcc")]
    public class ConditionalMove: InstructionWithDestinationAndSourceAndSize, IInstructionWithCondition {
        public ConditionalTestEnum Condition {
            get;
            set;
        }

        public override void WriteText(XSharp.Assembler.Assembler aAssembler, System.IO.TextWriter aOutput)
        {
            mMnemonic = "cmov" + Condition.GetMnemonic();
            base.WriteText(aAssembler, aOutput);
        }
    }
}