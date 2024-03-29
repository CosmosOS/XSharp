﻿namespace XSharp.Assembler.x86.x87
{
    [XSharp.Assembler.OpCode("FCMOVcc")]
    class FloatConditionalMove : InstructionWithDestinationAndSource
    {
        public FloatConditionalMoveTestEnum Condition
        {
            get;
            set;
        }

        public override void WriteText(XSharp.Assembler.Assembler aAssembler, System.IO.TextWriter aOutput)
        {
            switch (Condition)
            {
                case FloatConditionalMoveTestEnum.Below:
                    aOutput.Write("fcmovb");
                    break;
                case FloatConditionalMoveTestEnum.Equal:
                    aOutput.Write("fcmove");
                    break;
                case FloatConditionalMoveTestEnum.BelowOrEqual:
                    aOutput.Write("fcmovbe");
                    break;
                case FloatConditionalMoveTestEnum.Unordered:
                    aOutput.Write("fcmovu");
                    break;
                case FloatConditionalMoveTestEnum.NotBelow:
                    aOutput.Write("fcmovnb");
                    break;
                case FloatConditionalMoveTestEnum.NotEqual:
                    aOutput.Write("fcmovne");
                    break;
                case FloatConditionalMoveTestEnum.NotBelowOrEqual:
                    aOutput.Write("fcmovnbe");
                    break;
                case FloatConditionalMoveTestEnum.Ordered:
                    aOutput.Write("fcmovnu");
                    break;
            }
            aOutput.Write(" ");
            aOutput.Write("ST0");
            aOutput.Write(aAssembler.Separator);
            aOutput.Write(this.GetSourceAsString());
        }
    }
}
