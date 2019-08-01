; namespace DebugStub

; function Ping {
DebugStub_Ping:
    ; AL = 13
    Mov AL, 0xD
    ; ComWriteAL()
    Call DebugStub_ComWriteAL
; }

; function TraceOn {
DebugStub_TraceOn:
    ; .TraceMode = 1
    Mov DWORD [DebugStub_TraceMode], 0x1
; }

; function TraceOff {
DebugStub_TraceOff:
    ; .TraceMode = 0
    Mov DWORD [DebugStub_TraceMode], 0x0
; }
