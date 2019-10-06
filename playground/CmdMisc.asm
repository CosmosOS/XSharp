; namespace DebugStub

; function Ping {
DebugStub_Ping:
    ; Ds2Vs.Pong
    ; AL = 13
    Mov AL, 0xD
    ; ComWriteAL()
    Call DebugStub_ComWriteAL
; }
DebugStub_Ping_Exit:
Mov DWORD [INTS_LastKnownAddress], DebugStub_Ping_Exit
Ret 

; function TraceOn {
DebugStub_TraceOn:
    ; Tracing.On
    ; .TraceMode = 1
    Mov DWORD [DebugStub_TraceMode], 0x1
; }
DebugStub_TraceOn_Exit:
Mov DWORD [INTS_LastKnownAddress], DebugStub_TraceOn_Exit
Ret 

; function TraceOff {
DebugStub_TraceOff:
    ; Tracing.Off
    ; .TraceMode = 0
    Mov DWORD [DebugStub_TraceMode], 0x0
; }
DebugStub_TraceOff_Exit:
Mov DWORD [INTS_LastKnownAddress], DebugStub_TraceOff_Exit
Ret 
