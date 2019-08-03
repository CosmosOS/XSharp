

DebugStub_ComReadEAX:
		Call DebugStub_ComReadAL
		Ror EAX, 0x8
	DebugStub_ComReadEAX_Block1_End:

DebugStub_ComRead8:
    Call DebugStub_ComReadAL
    Mov BYTE [EDI], AL
    Add EDI, 0x1
DebugStub_ComRead16:
		Call DebugStub_ComRead8
	DebugStub_ComRead16_Block1_End:
DebugStub_ComRead32:
		Call DebugStub_ComRead8
	DebugStub_ComRead32_Block1_End:

DebugStub_ComWriteAL:
	Push ESI
    Push EAX
	Mov ESI, ESP
    Call DebugStub_ComWrite8
    Pop EAX
	Pop ESI
DebugStub_ComWriteAX:
    Push EAX
    Mov ESI, ESP
    Call DebugStub_ComWrite16
    Pop EAX
DebugStub_ComWriteEAX:
    Push EAX
    Mov ESI, ESP
    Call DebugStub_ComWrite32
    Pop EAX

DebugStub_ComWrite16:
	Call DebugStub_ComWrite8
	Call DebugStub_ComWrite8
DebugStub_ComWrite32:
	Call DebugStub_ComWrite8
	Call DebugStub_ComWrite8
	Call DebugStub_ComWrite8
	Call DebugStub_ComWrite8
DebugStub_ComWriteX:
DebugStub_ComWriteX_More:
	Call DebugStub_ComWrite8
	Dec ECX
