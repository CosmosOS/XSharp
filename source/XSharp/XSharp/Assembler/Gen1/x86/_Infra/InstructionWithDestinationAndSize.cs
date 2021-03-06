﻿namespace XSharp.Assembler.x86
{
    public abstract class InstructionWithDestinationAndSize : InstructionWithDestination, IInstructionWithSize {
        public InstructionWithDestinationAndSize(string mnemonic = null) : base(mnemonic)
        {
        }

        private byte mSize;
        public byte Size {
            get {
                this.DetermineSize(this, mSize);
                return mSize;
            }
            set {
                if (value > 0) {
                    SizeToString(value);
                }
                mSize = value;
            }
        }

        public override void WriteText( XSharp.Assembler.Assembler aAssembler, System.IO.TextWriter aOutput )
{
            aOutput.Write(mMnemonic);
            aOutput.Write(" ");
            aOutput.Write(SizeToString(Size));
            if (!DestinationEmpty)
            {
                aOutput.Write(" ");
                aOutput.Write(this.GetDestinationAsString());
            }
        }
    }
}
