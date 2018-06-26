using System;

namespace XSharp.Assembler
{
    public class DataEndIfDefined: DataMember, IEndIfDefined {
        public DataEndIfDefined()
            : base("define", Array.Empty<byte>()) {
        }

        public override void WriteText(XSharp.Assembler.Assembler aAssembler, System.IO.TextWriter aOutput)
        {
            aOutput.Write(this.GetAsText());
        }
    }
}
