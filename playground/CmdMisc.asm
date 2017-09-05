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
; }

; function TraceOff {
    ; Tracing.Off
    ; .TraceMode = 0
; }
