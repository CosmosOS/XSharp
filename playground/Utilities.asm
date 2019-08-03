; namespace DebugStub

; Uses EAX: expected difference.
; Modifies: EBX
; function CheckStack {
DebugStub_CheckStack:
    
    ; after a call, the stack gets pushed to, so add 4 to the expected difference
    ; eax += 4
    Add EAX, 0x4
    ; EBX = EBP
    Mov EBX, EBP
    ; EBX += EAX
    Add EBX, EAX

    ; if EBX != ESP {
        ; stack corruption.
        ; EAX = [ESP]
        Mov EAX, DWORD [ESP]
        ; .CallerEIP = EAX
        Mov DWORD [DebugStub_CallerEIP], EAX
        ; SendStackCorruptionOccurred()
        Call DebugStub_SendStackCorruptionOccurred
      ; halt:
      DebugStub_CheckStack_halt:
        ; goto halt
        Jmp DebugStub_CheckStack_halt
    ; }
    DebugStub_CheckStack_Block1_End:
; }
