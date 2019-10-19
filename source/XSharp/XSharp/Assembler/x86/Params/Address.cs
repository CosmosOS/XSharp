using System;

namespace XSharp.x86.Params
{
    public class MemoryAddress : Identifier
    {
        public override bool IsMatch(object aValue)
        {
            return aValue is Address address && address.AddressOf is string;
        }

        public override object Transform(object aValue)
        {
            return $"[{((Address)aValue).AddressOf}]";
        }
    }

    public class RegisterAddress : Param
    {
        public override bool IsMatch(object aValue)
        {
            return aValue is Address address && address.AddressOf is Register;
        }

        public override object Transform(object aValue)
        {
            var addr = aValue as Address;
            if (addr?.Offset == null)
            {
                return $"[{addr?.AddressOf}]";
            }
            else
            {
                var sign = addr.IsNegative ? "-" : "+";
                return $"[{addr.AddressOf} {sign} {addr.Offset}]";
            }
        }
    }

    /// <summary>
    /// Address is the object that contains an address
    /// of parameter which points to the actual object
    /// that should go inbetween brackets
    /// </summary>
    public class Address
    {
        public object AddressOf { get; private set; }
        public object Offset { get; }
        public bool IsNegative { get; }

        public Address(Register addressOf)
        {
            AddressOf = addressOf;
        }

        public Address(string label)
        {
            AddressOf = label;
        }

        public Address(Register baseIndex, object offset, bool isNegative)
        {
            if ((baseIndex.IsReg08 && offset is byte) || (baseIndex.IsReg16 && offset is UInt16) ||
                (baseIndex.IsReg32 && offset is UInt32))
            {
                AddressOf = baseIndex;
                Offset = offset;
                IsNegative = isNegative;
            }
            else
            {
                throw new Exception("Incompatible offset for the register type.");
            }
        }

        public Address AddPrefix(string prefix)
        {
            if (!(AddressOf is string))
            {
                throw new Exception("Prefix can only be added to a string type.");
            }
            AddressOf = $"{prefix}_{AddressOf}";
            return this;
        }
    }
}
