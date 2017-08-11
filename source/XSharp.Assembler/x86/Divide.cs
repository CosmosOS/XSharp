using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XSharp.Assembler.x86 {
	/// <summary>
	/// Puts the result of the divide into EAX, and the remainder in EDX
	/// </summary>
    [XSharp.Assembler.OpCode("div")]
	public class Divide: InstructionWithDestinationAndSize {
	}
}