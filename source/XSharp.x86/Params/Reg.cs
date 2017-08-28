using System;
using System.Collections.Generic;
using System.Text;

namespace XSharp.x86.Params {
    public abstract class Reg : Param {
        protected readonly int mSize;
        protected readonly bool mIsGenPurpose;

        public Reg(int aSize, bool aIsGenPurpose = true) {
            mSize = aSize;
            mIsGenPurpose = aIsGenPurpose;
        }

        public override bool IsMatch(object aValue) {
            var xReg = aValue as Register;
            if (xReg != null) {
                if (xReg.IsGenPurpose) {
                    return xReg.Size == mSize;
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
