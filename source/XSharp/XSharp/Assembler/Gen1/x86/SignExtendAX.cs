using System;

namespace XSharp.Assembler.x86
{
    [XSharp.Assembler.OpCode("cdq")]
    public class SignExtendAX : InstructionWithSize {
        public override void WriteText( XSharp.Assembler.Assembler aAssembler, System.IO.TextWriter aOutput )
        {
            switch (Size) {
                case 64:
                    aOutput.Write("cqo");
                    return;
                case 32:
                    aOutput.Write("cdq");
                    return;
                case 16:
                    aOutput.Write("cwde");
                    return;
                case 8:
                    aOutput.Write("cbw");
                    return;
                default:
                    throw new NotSupportedException();
            }
        }
    }
}
