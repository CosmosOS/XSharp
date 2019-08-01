; namespace DebugStub

; var AsmBreakEIP
DebugStub_AsmBreakEIP dd 0

; var AsmOrigByte
DebugStub_AsmOrigByte dd 0

; function DoAsmBreak {
DebugStub_DoAsmBreak:
	; ESI = .CallerESP
	Mov ESI, DWORD [DebugStub_CallerESP]
	; EAX = .AsmBreakEIP
	Mov EAX, DWORD [DebugStub_AsmBreakEIP]
	; [ESI-12] = EAX
	Mov DWORD [ESI - 12], EAX

	; ClearAsmBreak()
	Call DebugStub_ClearAsmBreak
  ; Break()
  Call DebugStub_Break
; }

; function SetAsmBreak {
DebugStub_SetAsmBreak:
	; ClearAsmBreak()
	Call DebugStub_ClearAsmBreak

  ; ComReadEAX()
  Call DebugStub_ComReadEAX
  ; .AsmBreakEIP = EAX
  Mov DWORD [DebugStub_AsmBreakEIP], EAX
  ; EDI = EAX
  Mov EDI, EAX

  ; AL = [EDI]
  Mov AL, BYTE [EDI]
  ; .AsmOrigByte = AL
  Mov BYTE [DebugStub_AsmOrigByte], AL

	; AL = $CC
	Mov AL, 0xCC
  ; [EDI] = AL
  Mov BYTE [EDI], AL
; }

; function ClearAsmBreak {
DebugStub_ClearAsmBreak:
  ; EDI = .AsmBreakEIP
  Mov EDI, DWORD [DebugStub_AsmBreakEIP]
  ; if EDI = 0 return
    
  ; AL = .AsmOrigByte
  Mov AL, BYTE [DebugStub_AsmOrigByte]
  ; [EDI] = AL
  Mov BYTE [EDI], AL

  ; .AsmBreakEIP = 0
  Mov DWORD [DebugStub_AsmBreakEIP], 0x0
; }

; function SetINT1_TrapFLAG {
DebugStub_SetINT1_TrapFLAG:
	; +EBP
	Push EBP
	; +EAX
	Push EAX

	; EBP = .CallerESP
	Mov EBP, DWORD [DebugStub_CallerESP]
	
	; EBP -= 4
	Sub EBP, 0x4
	; EAX = [EBP]
	Mov EAX, DWORD [EBP]
	; EAX | $0100
	; [EBP] = EAX
	Mov DWORD [EBP], EAX

	
	; -EAX
	Pop EAX
	; -EBP
	Pop EBP
; }

; function ResetINT1_TrapFLAG {
DebugStub_ResetINT1_TrapFLAG:
	; +EBP
	Push EBP
	; +EAX
	Push EAX

	; EBP = .CallerESP
	Mov EBP, DWORD [DebugStub_CallerESP]
	
	; EBP -= 4
	Sub EBP, 0x4
	; EAX = [EBP]
	Mov EAX, DWORD [EBP]
	; EAX & $FEFF
	; [EBP] = EAX
	Mov DWORD [EBP], EAX
	
	; -EAX
	Pop EAX
	; -EBP
	Pop EBP
; }
