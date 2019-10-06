; namespace DebugStub

; Location where INT3 has been injected.
; 0 if no INT3 is active.
; var AsmBreakEIP
DebugStub_AsmBreakEIP dd 0

; Old byte before INT3 was injected.
; Only 1 byte is used.
; var AsmOrigByte
DebugStub_AsmOrigByte dd 0

; function DoAsmBreak {
DebugStub_DoAsmBreak:
	; Since our Int3 is temp, we need to adjust return EIP to return to it, not after it.
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
DebugStub_DoAsmBreak_Exit:
Mov DWORD [INTS_LastKnownAddress], DebugStub_DoAsmBreak_Exit
Ret 

; function SetAsmBreak {
DebugStub_SetAsmBreak:
	; ClearAsmBreak()
	Call DebugStub_ClearAsmBreak

  ; ComReadEAX()
  Call DebugStub_ComReadEAX
  ; Save EIP of the break
  ; .AsmBreakEIP = EAX
  Mov DWORD [DebugStub_AsmBreakEIP], EAX
  ; EDI = EAX
  Mov EDI, EAX

  ; Save the old byte
  ; AL = [EDI]
  Mov AL, BYTE [EDI]
  ; .AsmOrigByte = AL
  Mov BYTE [DebugStub_AsmOrigByte], AL

  ; Inject INT3
	; Do in 2 steps to force a byte move to RAM (till X# can do byte in one step)
	; AL = $CC
	Mov AL, 0xCC
  ; [EDI] = AL
  Mov BYTE [EDI], AL
; }
DebugStub_SetAsmBreak_Exit:
Mov DWORD [INTS_LastKnownAddress], DebugStub_SetAsmBreak_Exit
Ret 

; function ClearAsmBreak {
DebugStub_ClearAsmBreak:
  ; EDI = .AsmBreakEIP
  Mov EDI, DWORD [DebugStub_AsmBreakEIP]
  ; If 0, we don't need to clear an older one.
  ; if EDI = 0 return
  Cmp EDI, 0x0
  Je DebugStub_ClearAsmBreak_Exit
    
	; Clear old break point and set back to original opcode / partial opcode
  ; AL = .AsmOrigByte
  Mov AL, BYTE [DebugStub_AsmOrigByte]
  ; [EDI] = AL
  Mov BYTE [EDI], AL

  ; .AsmBreakEIP = 0
  Mov DWORD [DebugStub_AsmBreakEIP], 0x0
; }
DebugStub_ClearAsmBreak_Exit:
Mov DWORD [INTS_LastKnownAddress], DebugStub_ClearAsmBreak_Exit
Ret 

; function SetINT1_TrapFLAG {
DebugStub_SetINT1_TrapFLAG:
	; Push EAX to make sure whatever we do below doesn't affect code afterwards
	; +EBP
	Push EBP
	; +EAX
	Push EAX

	; Set base pointer to the caller ESP
	; EBP = .CallerESP
	Mov EBP, DWORD [DebugStub_CallerESP]
	
	; Set the Trap Flag (http://en.wikipedia.org/wiki/Trap_flag)
	; For EFLAGS we want - the interrupt frame = ESP + 12
	; - The interrupt frame - 8 for correct byte = ESP + 12 - 8 = ESP + 4
	; - Therefore, ESP - 4 to get to the correct position
	; EBP -= 4
	Sub EBP, 0x4
	; EAX = [EBP]
	Mov EAX, DWORD [EBP]
	; EAX | $0100
	Or EAX, 0x100
	; [EBP] = EAX
	Mov DWORD [EBP], EAX

	; Restore the base pointer
	
	; Pop EAX - see +EAX at start of method
	; -EAX
	Pop EAX
	; -EBP
	Pop EBP
; }
DebugStub_SetINT1_TrapFLAG_Exit:
Mov DWORD [INTS_LastKnownAddress], DebugStub_SetINT1_TrapFLAG_Exit
Ret 

; function ResetINT1_TrapFLAG {
DebugStub_ResetINT1_TrapFLAG:
	; Push EAX to make sure whatever we do below doesn't affect code afterwards
	; +EBP
	Push EBP
	; +EAX
	Push EAX

	; Set base pointer to the caller ESP
	; EBP = .CallerESP
	Mov EBP, DWORD [DebugStub_CallerESP]
	
	; Clear the Trap Flag (http://en.wikipedia.org/wiki/Trap_flag)
	; See comment in SetINT1_TrapFlag
	; EBP -= 4
	Sub EBP, 0x4
	; EAX = [EBP]
	Mov EAX, DWORD [EBP]
	; EAX & $FEFF
	And EAX, 0xFEFF
	; [EBP] = EAX
	Mov DWORD [EBP], EAX
	
	; Pop EAX - see +EAX at start of method
	; -EAX
	Pop EAX
	; -EBP
	Pop EBP
; }
DebugStub_ResetINT1_TrapFLAG_Exit:
Mov DWORD [INTS_LastKnownAddress], DebugStub_ResetINT1_TrapFLAG_Exit
Ret 
