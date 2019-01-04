namespace XSharp.x86.Params
{
    public abstract class Reg : Param {
        protected Reg(int aSize, bool aIsGenPurpose = true) {
            Size = aSize;
            IsGenPurpose = aIsGenPurpose;
        }

        protected int Size { get; }
        protected bool IsGenPurpose { get; }

        public override bool IsMatch(object aValue) {
            if (aValue is Register xReg) {
                if (xReg.IsGenPurpose) {
                    return xReg.Size == Size;
                }
                // non gp - prob need to check name or other
            }
            return false;
        }
    }

    public class Reg08 : Reg {
        public Reg08() : base (8) { }
    }

    public class Reg16 : Reg {
        public Reg16() : base (16) { }
    }

    public class Reg32 : Reg {
        public Reg32() : base (32) { }
    }
}
