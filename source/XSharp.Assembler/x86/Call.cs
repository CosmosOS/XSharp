using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XSharp.Assembler.x86 {
    [XSharp.Assembler.OpCode("Call")]
	public class Call: JumpBase {
        public Call():base("Call") {
            mNear = false;
        }
	}
}
