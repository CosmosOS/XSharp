using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XSharp.x86 {
    public class Register {
        public class Names {
            public static readonly string[] Reg08 = "AH,AL,BH,BL,CH,CL,DH,DL".Split(',');
            public static readonly string[] Reg16 = "AX,BX,CX,DX".Split(',');
            public static readonly string[] Reg32 = "EAX,EBX,ECX,EDX,ESI,EDI".Split(',');
        }

        // These statics are not used much now but will be used even with NASM
        // and will become far important wtih binary assembler.
        public static Register EAX = new Register("EAX");
        public static Register EBX = new Register("EBX");
        public static Register ECX = new Register("ECX");
        public static Register EDX = new Register("EDX");
        public static Register ESI = new Register("ESI");
        public static Register EDI = new Register("EDI");
        //
        public static Register AX = new Register("AX");
        public static Register BX = new Register("BX");
        public static Register CX = new Register("CX");
        public static Register DX = new Register("DX");
        //
        public static Register AH = new Register("AH");
        public static Register AL = new Register("AL");
        public static Register BH = new Register("BH");
        public static Register BL = new Register("BL");
        public static Register CH = new Register("CH");
        public static Register CL = new Register("CL");
        public static Register DH = new Register("DH");
        public static Register DL = new Register("DL");

        public readonly bool IsGenPurpose;
        public readonly string Name;
        public readonly int Size;

        public Register(string aName) {
            Name = aName.ToUpper();

            //TODO Add special registers and leave IsGenPurpose = false
            if (Names.Reg32.Contains(Name)) {
                IsGenPurpose = true;
                Size = 32;
            } else if (Names.Reg16.Contains(Name)) {
                IsGenPurpose = true;
                Size = 16;
            } else if (Names.Reg08.Contains(Name)) {
                IsGenPurpose = true;
                Size = 8;
            } else {
                throw new Exception(aName + " is not a recognized register name.");
            }
        }

        public override string ToString() {
            return Name;
        }
    }
}
