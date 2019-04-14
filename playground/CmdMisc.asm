; namespace DebugStub

; function Ping {
DebugStub_Ping:
    ; Ds2Vs.Pong
    ; AL = 13
    Mov AL, 0xD
    ; ComWriteAL()
    Call DebugStub_ComWriteAL
; }

; function TraceOn {
DebugStub_TraceOn:
    ; Tracing.On
    ; .TraceMode = 1
    Mov DWORD [DebugStub_Var_TraceMode], 0x1
; }

; function TraceOff {
DebugStub_TraceOff:
    ; Tracing.Off
    ; .TraceMode = 0
    Mov DWORD [DebugStub_Var_TraceMode], 0x0
; }
