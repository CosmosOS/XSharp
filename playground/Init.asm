
DebugStub_MaxBPId dd 0

DebugStub_Init:
    Call DebugStub_Cls
    Call DebugStub_DisplayWaitMsg
    Call DebugStub_InitSerial
    Call DebugStub_WaitForDbgHandshake
    Call DebugStub_Cls

DebugStub_WaitForSignature:
    Mov EBX, 0x0
		Call DebugStub_ComReadAL
		Mov BL, AL
		Ror EBX, 0x8
	DebugStub_WaitForSignature_Block1_End:

DebugStub_WaitForDbgHandshake:
    Mov AL, 0x0
    Call DebugStub_ComWriteAL
	Mov AL, 0x0
    Call DebugStub_ComWriteAL
	Mov AL, 0x0
    Call DebugStub_ComWriteAL

	Push DebugStub_Const_Signature
    Mov ESI, ESP

    Call DebugStub_ComWrite32

    Pop EAX

    Mov AL, DebugStub_Const_Ds2Vs_Started
    Call DebugStub_ComWriteAL

    Call DebugStub_WaitForSignature
    Call DebugStub_ProcessCommandBatch
	Call DebugStub_Hook_OnHandshakeCompleted

%ifndef Exclude_Dummy_Hooks
DebugStub_Hook_OnHandshakeCompleted:
%endif
