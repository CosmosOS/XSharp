
DebugStub_CheckStack:
    
    Add EAX, 0x4
    Mov EBX, EBP
    Add EBX, EAX

        Mov EAX, DWORD [ESP]
        Mov DWORD [DebugStub_CallerEIP], EAX
        Call DebugStub_SendStackCorruptionOccurred
      DebugStub_CheckStack_halt:
    DebugStub_CheckStack_Block1_End:
