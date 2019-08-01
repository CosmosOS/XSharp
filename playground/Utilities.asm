; namespace DebugStub

; function CheckStack {
DebugStub_CheckStack:
    
    ; eax += 4
    Add EAX, 0x4
    ; EBX = EBP
    Mov EBX, EBP
    ; EBX += EAX
    Add EBX, EAX

    ; if EBX != ESP {
        ; EAX = [ESP]
        Mov EAX, DWORD [ESP]
        ; .CallerEIP = EAX
        Mov DWORD [DebugStub_CallerEIP], EAX
        ; SendStackCorruptionOccurred()
        Call DebugStub_SendStackCorruptionOccurred
      ; halt:
      DebugStub_halt:
        ; goto halt
    ; }
; }
