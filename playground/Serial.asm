

%ifndef Exclude_IOPort_Based_SerialInit

DebugStub_InitSerial:
  Mov DX, 0x1
	Mov AL, 0x0
  Call DebugStub_WriteRegister

	Mov DX, 0x3
	Mov AL, 0x80
	Call DebugStub_WriteRegister

	Mov DX, 0x0
	Mov AL, 0x1
	Call DebugStub_WriteRegister

	Mov DX, 0x1
	Mov AL, 0x0
	Call DebugStub_WriteRegister

	Mov DX, 0x3
	Mov AL, 0x3
	Call DebugStub_WriteRegister

  Mov DX, 0x2
	Mov AL, 0xC7
	Call DebugStub_WriteRegister

	Mov DX, 0x4
	Mov AL, 0x3
	Call DebugStub_WriteRegister

DebugStub_ComReadAL:
	Mov DX, 0x5
DebugStub_ComReadAL_Wait:
    Call DebugStub_ReadRegister
    Test AL, 0x1

	Mov DX, 0x0
  Call DebugStub_ReadRegister

DebugStub_ComWrite8:

	Mov DX, 0x5

DebugStub_ComWrite8_Wait:
    Call DebugStub_ReadRegister
	  Test AL, 0x20

	Mov DX, 0x0
  Mov AL, BYTE [ESI]
	Call DebugStub_WriteRegister

	Inc ESI

%endif
