using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XSharp.Assembler.x86 {
    [XSharp.Assembler.OpCode("stos")]
    public class Stos : InstructionWithSize, IInstructionWithPrefix {

        public InstructionPrefixes Prefixes {
            get;
            set;
        }

        public override void WriteText( XSharp.Assembler.Assembler aAssembler, System.IO.TextWriter aOutput )
        {
            if ((Prefixes & InstructionPrefixes.Repeat) != 0) {
                aOutput.Write("rep ");
            }
            switch (Size) {
                case 32:
                    aOutput.Write("stosd");
                    return;
                case 16:
                    aOutput.Write("stosw");
                    return;
                case 8:
                    aOutput.Write("stosb");
                    return;
                default: throw new Exception("Size not supported!");
            }
        }
    }
}