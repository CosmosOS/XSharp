; namespace DebugStub

; AX = [EBX]
Mov AX, WORD [EBX]
; ESI = @.CallerESP
Mov ESI, DebugStub_Const_CallerESP
; EAX = @.CallerESP
Mov EAX, DebugStub_Const_CallerESP
; AX = @.CallerESP
Mov AX, DebugStub_Const_CallerESP
; ESI = .CallerESP
Mov ESI, DWORD [DebugStub_Var_CallerESP]
; [ESI] = $00
Mov DWORD [ESI], 0x0


