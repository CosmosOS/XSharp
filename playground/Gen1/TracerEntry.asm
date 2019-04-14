; Generated at 4/14/2019 1:59:47 AM



DebugStub_TracerEntry:
cli
Pushad
mov dword [DebugStub_PushAllPtr], ESP
mov dword [DebugStub_CallerEBP], EBP
mov EBP, ESP
add EBP, 0x20
mov EAX, dword [EBP]
add EBP, 0xC
mov dword [DebugStub_CallerESP], EBP
mov EBX, EAX
MOV EAX, DR6
and EAX, 0x4000
cmp EAX, 0x4000
JE near DebugStub_TracerEntry_Block1_End
dec dword EBX

DebugStub_TracerEntry_Block1_End:
mov EAX, EBX
mov dword [DebugStub_CallerEIP], EAX
Call DebugStub_Executing
Popad
sti

DebugStub_TracerEntry_Exit:
iret
