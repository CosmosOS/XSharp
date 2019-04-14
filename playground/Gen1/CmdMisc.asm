; Generated at 4/14/2019 1:59:47 AM



DebugStub_Ping:
mov AL, 0xD
Call DebugStub_ComWriteAL

DebugStub_Ping_Exit:
mov dword [INTs_LastKnownAddress], DebugStub_Ping_Exit
Ret


DebugStub_TraceOn:
mov dword [DebugStub_TraceMode], 0x1

DebugStub_TraceOn_Exit:
mov dword [INTs_LastKnownAddress], DebugStub_TraceOn_Exit
Ret


DebugStub_TraceOff:
mov dword [DebugStub_TraceMode], 0x0

DebugStub_TraceOff_Exit:
mov dword [INTs_LastKnownAddress], DebugStub_TraceOff_Exit
Ret

