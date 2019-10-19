using XSharp.x86;

namespace XSharp.Tokens
{
    public class ZeroParamOp : Spruce.Tokens.AlphaNumList {
        protected OpCode mOpCode;

        protected ZeroParamOp(string aText, OpCode aOpCode) : base(aText) {
            mOpCode = aOpCode;
        }

        protected override object Transform(string aText) {
            return mOpCode;
        }
    }

    public class NOP : ZeroParamOp {
        public NOP() : base("NOP", OpCode.NOP) {
        }
    }

    public class Return : ZeroParamOp {
        public Return() : base("Return", OpCode.Ret) {
        }
    }

    public class PushAll : ZeroParamOp {
        public PushAll() : base("+All", OpCode.PushAD) {
        }
    }

    public class PopAll : ZeroParamOp {
        public PopAll() : base("-All", OpCode.PopAD) {
        }
    }
}
