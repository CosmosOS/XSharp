using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XSharp.Assembler.x86 {
    public interface IInstructionWithSize {
        byte Size {
            get;
            set;
        }
    }
}