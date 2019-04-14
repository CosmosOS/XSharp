; Generated at 4/14/2019 1:59:47 AM

DebugStub_DebugBPs dd 0
DebugStub_MaxBPId dd 0


DebugStub_Init:
Call DebugStub_Cls
Call DebugStub_DisplayWaitMsg
Call DebugStub_InitSerial
Call DebugStub_WaitForDbgHandshake
Call DebugStub_Cls

DebugStub_Init_Exit:
mov dword [INTs_LastKnownAddress], DebugStub_Init_Exit
Ret


DebugStub_WaitForSignature:
mov EBX, 0x0

DebugStub_WaitForSignature_Block1_Begin:
cmp EBX, DebugStub_Const_Signature
JE near DebugStub_WaitForSignature_Block1_End
Call DebugStub_ComReadAL
mov BL, AL
ror EBX, 0x8
Jmp DebugStub_WaitForSignature_Block1_Begin

DebugStub_WaitForSignature_Block1_End:

DebugStub_WaitForSignature_Exit:
mov dword [INTs_LastKnownAddress], DebugStub_WaitForSignature_Exit
Ret


DebugStub_WaitForDbgHandshake:
mov AL, 0x0
Call DebugStub_ComWriteAL
mov AL, 0x0
Call DebugStub_ComWriteAL
mov AL, 0x0
Call DebugStub_ComWriteAL
push dword DebugStub_Const_Signature
mov ESI, ESP
Call DebugStub_ComWrite32
pop dword EAX
mov AL, DebugStub_Const_Ds2Vs_Started
Call DebugStub_ComWriteAL
Call DebugStub_WaitForSignature
Call DebugStub_ProcessCommandBatch
Call DebugStub_Hook_OnHandshakeCompleted

DebugStub_WaitForDbgHandshake_Exit:
mov dword [INTs_LastKnownAddress], DebugStub_WaitForDbgHandshake_Exit
Ret

%ifndef Exclude_Dummy_Hooks

DebugStub_Hook_OnHandshakeCompleted:

DebugStub_Hook_OnHandshakeCompleted_Exit:
mov dword [INTs_LastKnownAddress], DebugStub_Hook_OnHandshakeCompleted_Exit
Ret

%endif
