; Generated at 4/14/2019 1:59:47 AM


%ifndef Exclude_IOPort_Based_SerialInit

DebugStub_InitSerial:
mov DX, 0x1
mov AL, 0x0
Call DebugStub_WriteRegister
mov DX, 0x3
mov AL, 0x80
Call DebugStub_WriteRegister
mov DX, 0x0
mov AL, 0x1
Call DebugStub_WriteRegister
mov DX, 0x1
mov AL, 0x0
Call DebugStub_WriteRegister
mov DX, 0x3
mov AL, 0x3
Call DebugStub_WriteRegister
mov DX, 0x2
mov AL, 0xC7
Call DebugStub_WriteRegister
mov DX, 0x4
mov AL, 0x3
Call DebugStub_WriteRegister

DebugStub_InitSerial_Exit:
mov dword [INTs_LastKnownAddress], DebugStub_InitSerial_Exit
Ret


DebugStub_ComReadAL:
mov DX, 0x5

DebugStub_ComReadAL_Wait:
Call DebugStub_ReadRegister
test AL, 0x1
JE near DebugStub_ComReadAL_Wait
mov DX, 0x0
Call DebugStub_ReadRegister

DebugStub_ComReadAL_Exit:
mov dword [INTs_LastKnownAddress], DebugStub_ComReadAL_Exit
Ret


DebugStub_ComWrite8:
mov DX, 0x5

DebugStub_ComWrite8_Wait:
Call DebugStub_ReadRegister
test AL, 0x20
JE near DebugStub_ComWrite8_Wait
mov DX, 0x0
mov AL, byte [ESI]
Call DebugStub_WriteRegister
inc dword ESI

DebugStub_ComWrite8_Exit:
mov dword [INTs_LastKnownAddress], DebugStub_ComWrite8_Exit
Ret

%endif
