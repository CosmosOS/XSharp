; Generated at 4/14/2019 1:59:47 AM



DebugStub_CheckStack:
add EAX, 0x4
mov EBX, EBP
add EBX, EAX
cmp EBX, ESP
JE near DebugStub_CheckStack_Block1_End
mov EAX, dword [ESP]
mov dword [DebugStub_CallerEIP], EAX
Call DebugStub_SendStackCorruptionOccurred

DebugStub_CheckStack_halt:
Jmp DebugStub_CheckStack_halt

DebugStub_CheckStack_Block1_End:

DebugStub_CheckStack_Exit:
mov dword [INTs_LastKnownAddress], DebugStub_CheckStack_Exit
Ret

