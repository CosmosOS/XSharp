using System;
using System.IO;

namespace XSharp.Assembler.x86
{
    public abstract class JumpBase : InstructionWithDestination
    {
        protected JumpBase(string mnemonic = null) : base(mnemonic)
        {
        }

        public string DestinationLabel
        {
            get => DestinationRef == null ? String.Empty : DestinationRef.Name;
            set => DestinationRef = ElementReference.New(value);
        }
        protected bool mNear = true;
        protected bool mCorrectAddress = true;
        protected virtual bool IsRelativeJump => true;

        public override void WriteData(XSharp.Assembler.Assembler aAssembler, Stream aOutput)
        {
            if (mCorrectAddress)
            {
                if (IsRelativeJump)
                {
                    if (DestinationValue.HasValue && !DestinationIsIndirect)
                    {
                        var xCurAddress = ActualAddress;
                        var xOrigValue = DestinationValue.Value;
                        DestinationValue = (uint)(xOrigValue - xCurAddress.Value);
                        try
                        {
                            base.WriteData(aAssembler, aOutput);
                            return;
                        }
                        finally
                        {
                            DestinationValue = xOrigValue;
                        }
                    }
                }
            }
            base.WriteData(aAssembler, aOutput);
        }
        //public override string ToString() {
        //    var xResult = base.ToString();
        //    if (mNear) {
        //        if (!xResult.StartsWith(Mnemonic + " near", StringComparison.InvariantCultureIgnoreCase)) {
        //            if (xResult.StartsWith(Mnemonic)) {
        //                return Mnemonic + " near " + xResult.Substring(Mnemonic.Length + 1);
        //            }
        //        }
        //    }
        //    return xResult;
        //}
    }
}
