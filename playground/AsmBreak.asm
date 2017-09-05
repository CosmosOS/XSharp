; namespace DebugStub

; Location where INT3 has been injected.
; 0 if no INT3 is active.
; var AsmBreakEIP

; Old byte before INT3 was injected.
; Only 1 byte is used.
; var AsmOrigByte

; function DoAsmBreak {
	; Since our Int3 is temp, we need to adjust return EIP to return to it, not after it.
	; ESI = .CallerESP
	Mov ESI, DWORD [DebugStub_Var_CallerESP]
	; EAX = .AsmBreakEIP
	Mov EAX, DWORD [DebugStub_Var_AsmBreakEIP]
	; [ESI-12] = EAX

	; ClearAsmBreak()
  ; Break()
; }

; function SetAsmBreak {
	; ClearAsmBreak()

  ; ComReadEAX()
  ; Save EIP of the break
  ; .AsmBreakEIP = EAX
  ; EDI = EAX
  Mov EDI, EAX

  ; Save the old byte
  ; AL = [EDI]
  Mov AL, BYTE [EDI]
  ; .AsmOrigByte = AL

  ; Inject INT3
	; Do in 2 steps to force a byte move to RAM (till X# can do byte in one step)
	; AL = $CC
	Mov AL, 0xCC
  ; [EDI] = AL
  Mov BYTE [EDI], AL
; }

; function ClearAsmBreak {
  ; EDI = .AsmBreakEIP
  Mov EDI, DWORD [DebugStub_Var_AsmBreakEIP]
  ; If 0, we don't need to clear an older one.
  ; if EDI = 0 return
    
	; Clear old break point and set back to original opcode / partial opcode
  ; AL = .AsmOrigByte
  Mov AL, BYTE [DebugStub_Var_AsmOrigByte]
  ; [EDI] = AL
  Mov BYTE [EDI], AL

  ; .AsmBreakEIP = 0
; }

; function SetINT1_TrapFLAG {
	; Push EAX to make sure whatever we do below doesn't affect code afterwards
	; +EBP
	Push EBP
	; +EAX
	Push EAX

	; Set base pointer to the caller ESP
	; EBP = .CallerESP
	Mov EBP, DWORD [DebugStub_Var_CallerESP]
	
	; Set the Trap Flag (http://en.wikipedia.org/wiki/Trap_flag)
	; For EFLAGS we want - the interrupt frame = ESP + 12
	; - The interrupt frame - 8 for correct byte = ESP + 12 - 8 = ESP + 4
	; - Therefore, ESP - 4 to get to the correct position
	; EBP -= 4
	; EAX = [EBP]
	Mov EAX, DWORD [EBP]
	; EAX | $0100
	; [EBP] = EAX
	Mov DWORD [EBP], EAX

	; Restore the base pointer
	
	; Pop EAX - see +EAX at start of method
	; -EAX
	Pop EAX
	; -EBP
	Pop EBP
; }

; function ResetINT1_TrapFLAG {
	; Push EAX to make sure whatever we do below doesn't affect code afterwards
	; +EBP
	Push EBP
	; +EAX
	Push EAX

	; Set base pointer to the caller ESP
	; EBP = .CallerESP
	Mov EBP, DWORD [DebugStub_Var_CallerESP]
	
	; Clear the Trap Flag (http://en.wikipedia.org/wiki/Trap_flag)
	; See comment in SetINT1_TrapFlag
	; EBP -= 4
	; EAX = [EBP]
	Mov EAX, DWORD [EBP]
	; EAX & $FEFF
	; [EBP] = EAX
	Mov DWORD [EBP], EAX
	
	; Pop EAX - see +EAX at start of method
	; -EAX
	Pop EAX
	; -EBP
	Pop EBP
; }
