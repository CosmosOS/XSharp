using System;

namespace XSharp.Assembler.x86
{
    public abstract class InstructionWithDestinationAndSource : InstructionWithDestination, IInstructionWithSource {

        public InstructionWithDestinationAndSource()
        {

        }

        public InstructionWithDestinationAndSource(string mnemonic):base(mnemonic)
        {

        }

        public XSharp.Assembler.ElementReference SourceRef {
            get;
            set;
        }

        public RegistersEnum? SourceReg
        {
            get;
            set;
        }

        public uint? SourceValue
        {
            get;
            set;
        }

        public bool SourceIsIndirect {
            get;
            set;
        }

        public bool SourceRequiresSize { get; set; }

        public int? SourceDisplacement {
            get;
            set;
        }

        public bool SourceEmpty
        {
            get;
            set;
        }
        protected string GetSourceAsString() {
            string xDest = "";
            //if ((SourceValue.HasValue || SourceRef != null) &&
            //    SourceIsIndirect && SourceReg != null) {
            //    throw new Exception("[Scale*index+base] style addressing not supported at the moment");
            //}
            if (SourceRef != null) {
                SourceIsIndirect = true;
                xDest = " rel "+SourceRef.ToString();
            } else {
                if (SourceReg != null) {
                    xDest = Registers.GetRegisterName(SourceReg.Value);
                } else {
                    if (SourceValue.HasValue)
                        xDest = "0x" + SourceValue.GetValueOrDefault().ToString("X").ToUpperInvariant();
                }
            }
            if (SourceDisplacement != null && SourceDisplacement != 0) {
              xDest += (SourceDisplacement < 0 ? " - " : " + ") + Math.Abs(SourceDisplacement.Value);
            }
            if (SourceIsIndirect && SourceRequiresSize)
            {
                return SizeToString(64) + " [" + xDest + "]";
            }
            else if (SourceIsIndirect)
            {
                return " [" + xDest + "]"; ;
            }
            else
            {
                return xDest;
            }
        }

        public override bool IsComplete( XSharp.Assembler.Assembler aAssembler )
        {
            if (SourceRef != null) {
                ulong xAddress;
                return base.IsComplete(aAssembler) && SourceRef.Resolve(aAssembler, out xAddress);
            }
            return base.IsComplete(aAssembler);
        }

        public override void UpdateAddress( XSharp.Assembler.Assembler aAssembler, ref ulong aAddress )
        {
            if (SourceRef != null) {
                SourceValue = 0xFFFFFFFF;
            }
            base.UpdateAddress(aAssembler, ref aAddress);
        }

        public override void WriteText( XSharp.Assembler.Assembler aAssembler, System.IO.TextWriter aOutput )
        {
            aOutput.Write(mMnemonic);
            String destination=this.GetDestinationAsString();
            if (!destination.Equals("")){
                aOutput.Write(" ");
                aOutput.Write(destination);
                string source = this.GetSourceAsString();
                if (!(SourceEmpty && source.Equals(""))){
                    aOutput.Write(aAssembler.Separator);
                    aOutput.Write(source);
                 }
            }
        }
    }
}
