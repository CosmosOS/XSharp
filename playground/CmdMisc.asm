
DebugStub_Ping:
    Mov AL, 0xD
    Call DebugStub_ComWriteAL

DebugStub_TraceOn:
    Mov DWORD [DebugStub_TraceMode], 0x1

DebugStub_TraceOff:
    Mov DWORD [DebugStub_TraceMode], 0x0
