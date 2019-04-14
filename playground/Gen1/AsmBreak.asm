; Generated at 4/14/2019 1:59:47 AM

DebugStub_AsmBreakEIP dd 0
DebugStub_AsmOrigByte dd 0


DebugStub_DoAsmBreak:
mov ESI, dword [DebugStub_CallerESP]
mov EAX, dword [DebugStub_AsmBreakEIP]
mov dword [ESI - 12], EAX
Call DebugStub_ClearAsmBreak
Call DebugStub_Break

DebugStub_DoAsmBreak_Exit:
mov dword [INTs_LastKnownAddress], DebugStub_DoAsmBreak_Exit
Ret


DebugStub_SetAsmBreak:
Call DebugStub_ClearAsmBreak
Call DebugStub_ComReadEAX
mov dword [DebugStub_AsmBreakEIP], EAX
mov EDI, EAX
mov AL, byte [EDI]
mov byte [DebugStub_AsmOrigByte], AL
mov AL, 0xCC
mov byte [EDI], AL

DebugStub_SetAsmBreak_Exit:
mov dword [INTs_LastKnownAddress], DebugStub_SetAsmBreak_Exit
Ret


DebugStub_ClearAsmBreak:
mov EDI, dword [DebugStub_AsmBreakEIP]
cmp EDI, 0x0
JE near DebugStub_ClearAsmBreak_Exit
mov AL, byte [DebugStub_AsmOrigByte]
mov byte [EDI], AL
mov dword [DebugStub_AsmBreakEIP], 0x0

DebugStub_ClearAsmBreak_Exit:
mov dword [INTs_LastKnownAddress], DebugStub_ClearAsmBreak_Exit
Ret


DebugStub_SetINT1_TrapFLAG:
push dword EBP
push dword EAX
mov EBP, dword [DebugStub_CallerESP]
sub EBP, 0x4
mov EAX, dword [EBP]
or EAX, 0x100
mov dword [EBP], EAX
pop dword EAX
pop dword EBP

DebugStub_SetINT1_TrapFLAG_Exit:
mov dword [INTs_LastKnownAddress], DebugStub_SetINT1_TrapFLAG_Exit
Ret


DebugStub_ResetINT1_TrapFLAG:
push dword EBP
push dword EAX
mov EBP, dword [DebugStub_CallerESP]
sub EBP, 0x4
mov EAX, dword [EBP]
and EAX, 0xFEFF
mov dword [EBP], EAX
pop dword EAX
pop dword EBP

DebugStub_ResetINT1_TrapFLAG_Exit:
mov dword [INTs_LastKnownAddress], DebugStub_ResetINT1_TrapFLAG_Exit
Ret

