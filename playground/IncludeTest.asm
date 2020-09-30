; namespace DebugStub

; include Test.xs
; namespace DebugStub

; .v1 = 1
Mov DWORD [DebugStub_v1], 0x1
; .v1 = AL
Mov BYTE [DebugStub_v1], AL
; .v1 = EAX
Mov DWORD [DebugStub_v1], EAX
; .v1 = #const
Mov [DebugStub_v1], DebugStub_Const_const

; .v = 4
Mov DWORD [DebugStub_v], 0x4
