using System;

namespace XSharp.Assembler.x86
{
    public abstract class InstructionWithDestination : Instruction, IInstructionWithDestination{
        public InstructionWithDestination()
        {

        }

        public InstructionWithDestination(string mnemonic):base(mnemonic)
        {

        }

        public XSharp.Assembler.ElementReference DestinationRef {
            get;
            set;
        }

        public RegistersEnum? DestinationReg
        {
            get;
            set;
        }

        public uint? DestinationValue
        {
            get;
            set;
        }

        public bool DestinationIsIndirect {
            get;
            set;
        }

        public int? DestinationDisplacement {
            get;
            set;
        }

        public bool DestinationEmpty
        {
            get;
            set;
        }

        public override bool IsComplete( XSharp.Assembler.Assembler aAssembler )
        {
            if (DestinationRef != null) {
                ulong xAddress;
                return base.IsComplete(aAssembler) && DestinationRef.Resolve(aAssembler, out xAddress);
            }
            return base.IsComplete(aAssembler);
        }

        public override void UpdateAddress( XSharp.Assembler.Assembler aAssembler, ref ulong aAddresss )
        {
            if (DestinationRef != null) {
                DestinationValue = 0xFFFFFFFF;
            }
            base.UpdateAddress(aAssembler, ref aAddresss);
        }

        public override void WriteText( XSharp.Assembler.Assembler aAssembler, System.IO.TextWriter aOutput )
        {
            aOutput.Write(mMnemonic);
            String destination = this.GetDestinationAsString();
            if (!(DestinationEmpty && destination.Equals("")))
            {
                aOutput.Write(" ");
                aOutput.Write(destination);
            }
        }
    }
}
