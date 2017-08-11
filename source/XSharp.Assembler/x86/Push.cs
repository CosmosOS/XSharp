using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XSharp.Assembler.x86 {
    [XSharp.Assembler.OpCode("push")]
    public class Push : InstructionWithDestinationAndSize {

        public Push():base("push") {
            Size = 32;
        }
    }
}
