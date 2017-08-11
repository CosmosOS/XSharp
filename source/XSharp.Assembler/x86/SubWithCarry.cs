using System;
using System.Linq;

namespace XSharp.Assembler.x86
{
	/// <summary>
	/// Subtracts the source operand from the destination operand and 
	/// replaces the destination operand with the result. 
	/// </summary>
    [XSharp.Assembler.OpCode("sbb")]
	public class SubWithCarry : InstructionWithDestinationAndSourceAndSize
	{
}
}
