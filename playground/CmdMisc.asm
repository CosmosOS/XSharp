; namespace DebugStub

; function Ping {
    ; Ds2Vs.Pong
    ; AL = 13
    Mov AL, 0xD
    ; ComWriteAL()
; }

; function TraceOn {
    ; Tracing.On
    ; .TraceMode = 1
    Mov DWORD [DebugStub_Var_TraceMode], 0x1
; }

; function TraceOff {
    ; Tracing.Off
    ; .TraceMode = 0
    Mov DWORD [DebugStub_Var_TraceMode], 0x0
; }
