namespace XSharp.Assembler.x86
{
    [XSharp.Assembler.OpCode("Call")]
	public class Call: JumpBase {
        public Call():base("Call") {
            mNear = false;
        }
	}
}
