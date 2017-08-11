using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XSharp.Assembler
{
    [XSharp.Assembler.OpCode("%else")]
    public class Else: Instruction
    {
        public override void WriteText(XSharp.Assembler.Assembler aAssembler, System.IO.TextWriter aOutput)
        {
            aOutput.Write(Mnemonic);
        }
    }
}
