
DebugStub_AsmBreakEIP dd 0

DebugStub_AsmOrigByte dd 0

DebugStub_DoAsmBreak:
	Mov ESI, DWORD [DebugStub_CallerESP]
	Mov EAX, DWORD [DebugStub_AsmBreakEIP]
	Mov DWORD [ESI - 12], EAX

	Call DebugStub_ClearAsmBreak
  Call DebugStub_Break

DebugStub_SetAsmBreak:
	Call DebugStub_ClearAsmBreak

  Call DebugStub_ComReadEAX
  Mov DWORD [DebugStub_AsmBreakEIP], EAX
  Mov EDI, EAX

  Mov AL, BYTE [EDI]
  Mov BYTE [DebugStub_AsmOrigByte], AL

	Mov AL, 0xCC
  Mov BYTE [EDI], AL

DebugStub_ClearAsmBreak:
  Mov EDI, DWORD [DebugStub_AsmBreakEIP]
    
  Mov AL, BYTE [DebugStub_AsmOrigByte]
  Mov BYTE [EDI], AL

  Mov DWORD [DebugStub_AsmBreakEIP], 0x0

DebugStub_SetINT1_TrapFLAG:
	Push EBP
	Push EAX

	Mov EBP, DWORD [DebugStub_CallerESP]
	
	Sub EBP, 0x4
	Mov EAX, DWORD [EBP]
	Mov DWORD [EBP], EAX

	
	Pop EAX
	Pop EBP

DebugStub_ResetINT1_TrapFLAG:
	Push EBP
	Push EAX

	Mov EBP, DWORD [DebugStub_CallerESP]
	
	Sub EBP, 0x4
	Mov EAX, DWORD [EBP]
	Mov DWORD [EBP], EAX
	
	Pop EAX
	Pop EBP
