

%ifndef Exclude_Memory_Based_Console

DebugStub_Const_VidBase equ 753664

DebugStub_Cls:
    Mov ESI, DebugStub_Const_VidBase

		Mov DWORD [ESI], 0x0
		Inc ESI

		Mov DWORD [ESI], 0xA
		Inc ESI
	DebugStub_Cls_Block1_End:

DebugStub_DisplayWaitMsg:
	Mov ESI, DebugStub_DebugWaitMsg

    Mov EDI, DebugStub_Const_VidBase
    Add EDI, 0x668

		Mov AL, BYTE [ESI]
		Mov BYTE [EDI], AL
		Inc ESI
		Add EDI, 0x2
	DebugStub_DisplayWaitMsg_Block1_End:

%endif
