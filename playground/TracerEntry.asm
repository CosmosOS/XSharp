


DebugStub_TracerEntry:

cli


	PushAD 
Mov DWORD [DebugStub_PushAllPtr], ESP
Mov DWORD [DebugStub_CallerEBP], EBP

Mov EBP, ESP
Add EBP, 0x20
Mov EAX, DWORD [EBP]

Add EBP, 0xC
Mov DWORD [DebugStub_CallerESP], EBP


Mov EBX, EAX
MOV EAX, DR6
	Dec EBX
DebugStub_TracerEntry_Block1_End:
Mov EAX, EBX

Mov DWORD [DebugStub_CallerEIP], EAX

	Call DebugStub_Executing

PopAD 

sti

