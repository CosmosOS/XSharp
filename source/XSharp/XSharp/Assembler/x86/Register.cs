using System;
using System.Linq;

namespace XSharp.x86
{
    public class Register
    {
        public static class Names
        {
            public static readonly string[] Reg08 = "AH,AL,BH,BL,CH,CL,DH,DL".Split(',');
            public static readonly string[] Reg16 = "AX,BX,CX,DX".Split(',');
            public static readonly string[] Reg32 = "EAX,EBX,ECX,EDX,ESP,EBP,ESI,EDI".Split(',');
            public static readonly string[] Reg64 = "RAX,RBX,RCX,RDX,RSP,RBP,RSI,RDI".Split(',');
        }

        // These statics are not used much now but will be used even with NASM
        // and will become far important wtih binary assembler.
        public static readonly Register EAX = new Register("EAX");

        public static readonly Register EBX = new Register("EBX");
        public static readonly Register ECX = new Register("ECX");
        public static readonly Register EDX = new Register("EDX");
        public static readonly Register ESP = new Register("ESP");
        public static readonly Register EBP = new Register("EBP");
        public static readonly Register ESI = new Register("ESI");

        public static readonly Register EDI = new Register("EDI");

        //
        public static readonly Register AX = new Register("AX");

        public static readonly Register BX = new Register("BX");
        public static readonly Register CX = new Register("CX");

        public static readonly Register DX = new Register("DX");

        //
        public static readonly Register AH = new Register("AH");

        public static readonly Register AL = new Register("AL");
        public static readonly Register BH = new Register("BH");
        public static readonly Register BL = new Register("BL");
        public static readonly Register CH = new Register("CH");
        public static readonly Register CL = new Register("CL");
        public static readonly Register DH = new Register("DH");
        public static readonly Register DL = new Register("DL");

        public bool IsGenPurpose { get; }
        public string Name { get; }
        public int Size { get; }

        public Register(string aName)
        {
            Name = aName.ToUpper();

            //TODO Add special registers and leave IsGenPurpose = false
            if (Names.Reg64.Contains(Name))
            {
                IsGenPurpose = true;
                Size = 64;
            }
            else if (Names.Reg32.Contains(Name))
            {
                IsGenPurpose = true;
                Size = 32;
            }
            else if (Names.Reg16.Contains(Name))
            {
                IsGenPurpose = true;
                Size = 16;
            }
            else if (Names.Reg08.Contains(Name))
            {
                IsGenPurpose = true;
                Size = 8;
            }
            else
            {
                throw new Exception(aName + " is not a recognized register name.");
            }
        }

        public bool CheckIs(string aValidRegs, bool throwException = false)
        {
            if (!(aValidRegs + ",").Contains(Name + ","))
            {
                if (throwException)
                {
                    throw new Exception("Invalid register: {Name}.\r\nMust be one of: {aValidRegs}");
                }

                return false;
            }
            return true;
        }

        public void CheckIsDX()
        {
            CheckIs("DX", true);
        }

        public void CheckIsAccumulator()
        {
            CheckIs("EAX,AX,AL", true);
        }

        public bool IsReg08 => Names.Reg08.Contains(Name);

        public bool IsReg16 => Names.Reg16.Contains(Name);

        public bool IsReg32 => Names.Reg32.Contains(Name);

        public string RegSize => IsReg08
            ? "byte"
            : IsReg16
                ? "word"
                : IsReg32
                    ? "dword"
                    : throw new NotSupportedException("Unknown register size");

        public override string ToString()
        {
            return Name;
        }
    }
}
