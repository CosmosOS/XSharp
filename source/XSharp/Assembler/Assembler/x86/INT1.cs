namespace XSharp.Assembler.x86 {

  // See note in Int3 as to why we need a separate op for Int1 versus Int 0x01
  [XSharp.Assembler.OpCode("Int1")]
	public class INT1: Instruction { 
  }

}
