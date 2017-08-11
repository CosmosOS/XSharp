using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XSharp.Assembler.x86 {
    [XSharp.Assembler.OpCode("jmp")]
    public class JumpToSegment : Instruction {
        public XSharp.Assembler.ElementReference DestinationRef {
            get;
            set;
        }

        public ushort Segment {
            get;
            set;
        }

        public override void WriteText( XSharp.Assembler.Assembler aAssembler, System.IO.TextWriter aOutput )
        {
                if (DestinationRef != null) {
                    aOutput.Write("jmp ");
                    aOutput.Write(Segment);
                    aOutput.Write(":");
                    aOutput.Write(DestinationRef.ToString());
                } else {
                    aOutput.Write("jmp ");
                    aOutput.Write(Segment);
                    aOutput.Write(":0x0");
                }
        }

        public string DestinationLabel {
            get {
                if (DestinationRef != null) {
                    return DestinationRef.Name;
                }
                return String.Empty;
            }
            set {
                DestinationRef = XSharp.Assembler.ElementReference.New(value);
            }
        }

        public override bool IsComplete( XSharp.Assembler.Assembler aAssembler )
        {
            ulong xAddress;
            return DestinationRef == null || DestinationRef.Resolve(aAssembler, out xAddress);
        }

        public override void UpdateAddress(XSharp.Assembler.Assembler aAssembler, ref ulong aAddress) {
            base.UpdateAddress(aAssembler, ref aAddress);
            aAddress += 7;
        }

        //public override byte[] GetData(Assembler aAssembler) {
        public override void WriteData( XSharp.Assembler.Assembler aAssembler, System.IO.Stream aOutput )
        {
            aOutput.WriteByte(0xEA);
            ulong xAddress = 0;
            if (DestinationRef != null && DestinationRef.Resolve(aAssembler, out xAddress)) {
                xAddress = (ulong)(((long)xAddress) + DestinationRef.Offset);
            }
            aOutput.Write(BitConverter.GetBytes((uint)(xAddress)), 0, 4);
            aOutput.Write(BitConverter.GetBytes(Segment), 0, 2);
        }
    }
}