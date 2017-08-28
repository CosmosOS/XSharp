using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XSharp.x86 {
    public class Register {
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

            if (Params.Reg32.Names.Contains(Name)) {
                IsGenPurpose = true;
            } else if (Params.Reg16.Names.Contains(Name)) {
                IsGenPurpose = true;
            } else if (Params.Reg08.Names.Contains(Name)) {
                IsGenPurpose = true;
            } else {
                //TODO Add special registers and leave IsGenPurpose = false
                throw new Exception(aName + " is not a recognized register name.");
            }
        }

        public override string ToString() {
            return Name;
        }
    }
}
