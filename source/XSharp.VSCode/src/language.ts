import { EOL } from 'os';

export class Platform {
    private static readonly Register8 = "8 bit register.";
    private static readonly Register16 = "16 bit register.";
    private static readonly Register32 = "32 bit register.";
    private static readonly LinkRegister = "Link register. Holds the address to return when a function completes.";
    private static readonly ProgramCounter = "Program counter register.";
    private static readonly StackPointer = "Stack pointer register.";

    public static readonly x86 = new Platform(
        [
            { Name: "EAX", Description: Platform.Register32 }, { Name: "AX", Description: Platform.Register16 },
            { Name: "AH", Description: Platform.Register8 }, { Name: "AL", Description: Platform.Register8 },
            { Name: "EBX", Description: Platform.Register32 }, { Name: "BX", Description: Platform.Register16 },
            { Name: "BH", Description: Platform.Register8 }, { Name: "BL", Description: Platform.Register8 },
            { Name: "ECX", Description: Platform.Register32 }, { Name: "CX", Description: Platform.Register16 },
            { Name: "CH", Description: Platform.Register8 }, { Name: "CL", Description: Platform.Register8 },
            { Name: "EDX", Description: Platform.Register32 }, { Name: "DX", Description: Platform.Register16 },
            { Name: "DH", Description: Platform.Register8 }, { Name: "DL", Description: Platform.Register8 },
            { Name: "ESI", Description: Platform.Register32 }, { Name: "EDI", Description: Platform.Register32 },
            { Name: "EBP", Description: Platform.Register32 }, { Name: "ESP", Description: Platform.StackPointer }
        ]
    );

    //public static x86_64 = new Platform();

    public static ARMv7 = new Platform(
        [
            { Name: "r0", Description: Platform.Register32 }, { Name: "r1", Description: Platform.Register32 },
            { Name: "r2", Description: Platform.Register32 }, { Name: "r3", Description: Platform.Register32 },
            { Name: "r4", Description: Platform.Register32 }, { Name: "r5", Description: Platform.Register32 },
            { Name: "r6", Description: Platform.Register32 }, { Name: "r7", Description: Platform.Register32 },
            { Name: "r8", Description: Platform.Register32 }, { Name: "r9", Description: Platform.Register32 },
            { Name: "r10", Description: Platform.Register32 }, { Name: "r11", Description: Platform.Register32 },
            { Name: "r12", Description: Platform.Register32 }, { Name: "sp", Description: Platform.StackPointer },
            { Name: "pc", Description: Platform.ProgramCounter }, { Name: "lr", Description: Platform.LinkRegister }
        ]
    );

    //public static ARMv8 = new Platform();

    private mRegisters: Register[];

    public get Registers(): Register[] {
        return this.mRegisters;
    }

    private constructor(registers: Register[]) {
        this.mRegisters = registers;
    }
}

export class Register {
    Name: string;
    Description: string;
}

export class Operator {
    Symbol: string;
    Description: string;
}

export const Operators: Operator[] = [
    { Symbol: "+", Description: "Adds the right operand to the left operand.  " + EOL + "OR  " + EOL + "Pushes a register: **+**EAX -> push eax" },
    { Symbol: "-", Description: "Subtracts the right operand to the left operand.  " + EOL + "OR  " + EOL + "Pops a register: **-**EAX -> pop eax" },
    { Symbol: "*", Description: "Multiplies the left operand by the right operand." },
    { Symbol: "[", Description: "**[**register**]** -> represents the value at the memory address in register  " + EOL + "register**[**number**]** -> represents the value at memory address in register + number" },
    { Symbol: "]", Description: "**[**register**]** -> represents the value at the memory address in register  " + EOL + "register**[**number**]** -> represents the value at memory address in register + number" },
    { Symbol: "<", Description: "Unsigned less than." },
    { Symbol: "<=", Description: "Unsigned less than or equal." },
    { Symbol: ">", Description: "Unsigned greater than." },
    { Symbol: ">=", Description: "Unsigned greater than or equal." },
    { Symbol: "=", Description: "Equals." },
    { Symbol: "!=", Description: "Not equals." },
    { Symbol: "0", Description: "Zero." },
    { Symbol: "!0", Description: "Not zero." },
    { Symbol: ">>", Description: "Shift right." },
    { Symbol: "<<", Description: "Shift left." },
    { Symbol: "~>", Description: "Rotate right." },
    { Symbol: "<~", Description: "Rotate left." }
]
