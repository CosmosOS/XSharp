
DebugStub_SendRegisters:
    Mov AL, DebugStub_Const_Ds2Vs_Registers
    Call DebugStub_ComWriteAL

    Mov ESI, DWORD [DebugStub_PushAllPtr]
    Mov ECX, 0x20
    Call DebugStub_ComWriteX

    Mov ESI, DebugStub_CallerESP
    Call DebugStub_ComWrite32

    Mov ESI, DebugStub_CallerEIP
    Call DebugStub_ComWrite32

DebugStub_SendFrame:
    Mov AL, DebugStub_Const_Ds2Vs_Frame
    Call DebugStub_ComWriteAL

    Mov EAX, 0x20
    Call DebugStub_ComWriteAX

    Mov ESI, DWORD [DebugStub_CallerEBP]
    Add ESI, 0x8
    Mov ECX, 0x20
    Call DebugStub_ComWriteX

DebugStub_SendCommandOnChannel:
  PushAD 
    Call DebugStub_ComWriteAL
  PopAD 

  Mov AL, BL

  PushAD 
    Call DebugStub_ComWriteAL
  PopAD 

  PushAD 
    Mov EAX, ECX
    Call DebugStub_ComWriteEAX
  PopAD 

        Call DebugStub_ComWrite8
        Dec ECX
    DebugStub_SendCommandOnChannel_Block1_End:

DebugStub_SendStack:
    Mov AL, DebugStub_Const_Ds2Vs_Stack
    Call DebugStub_ComWriteAL

    Mov ESI, DWORD [DebugStub_CallerESP]
    Mov EAX, DWORD [DebugStub_CallerEBP]
    Sub EAX, ESI
    Call DebugStub_ComWriteAX

    Mov ESI, DWORD [DebugStub_CallerESP]
        Call DebugStub_ComWrite8
    DebugStub_SendStack_Block1_End:

DebugStub_SendMethodContext:
    PushAD 

    Mov AL, DebugStub_Const_Ds2Vs_MethodContext
    Call DebugStub_ComWriteAL

    Mov ESI, DWORD [DebugStub_CallerEBP]

    Call DebugStub_ComReadEAX
    Add ESI, EAX
    Call DebugStub_ComReadEAX
    Mov ECX, EAX


        Call DebugStub_ComWrite8
        Dec ECX
    DebugStub_SendMethodContext_Block1_End:

DebugStub_SendMethodContext_Exit:
    PopAD 

DebugStub_SendMemory:
    PushAD 

    Mov AL, DebugStub_Const_Ds2Vs_MemoryData
    Call DebugStub_ComWriteAL

    Call DebugStub_ComReadEAX
    Mov ESI, EAX
    Call DebugStub_ComReadEAX
    Mov ECX, EAX

        Call DebugStub_ComWrite8
        Dec ECX
    DebugStub_SendMemory_Block1_End:

DebugStub_SendMemory_Exit:
    PopAD 

DebugStub_SendTrace:
    Mov AL, DebugStub_Const_Ds2Vs_BreakPoint
        Mov AL, DebugStub_Const_Ds2Vs_TracePoint
    DebugStub_SendTrace_Block1_End:
    Call DebugStub_ComWriteAL

    Mov ESI, DebugStub_CallerEIP
    Call DebugStub_ComWrite32

DebugStub_SendText:
Push EBP
Mov EBP, ESP
    PushAD 
    Mov AL, DebugStub_Const_Ds2Vs_Message
    Call DebugStub_ComWriteAL

    Mov ESI, EBP
    Add ESI, 0xC
    Mov ECX, DWORD [ESI]
    Call DebugStub_ComWrite16

    Mov ESI, DWORD [EBP + 8]
DebugStub_SendText_WriteChar:
    Call DebugStub_ComWrite8
    Dec ECX
    Inc ESI

DebugStub_SendText_Finalize:
    PopAD 
  Pop EBP

DebugStub_SendSimpleNumber:
Push EBP
Mov EBP, ESP
    PushAD 
    Mov AL, DebugStub_Const_Ds2Vs_SimpleNumber
    Call DebugStub_ComWriteAL

    Mov EAX, DWORD [EBP + 8]
    Call DebugStub_ComWriteEAX

    PopAD 
  Pop EBP

DebugStub_SendKernelPanic:
Push EBP
Mov EBP, ESP
    PushAD 
    Mov AL, DebugStub_Const_Ds2Vs_KernelPanic
    Call DebugStub_ComWriteAL

    Mov EAX, DWORD [EBP + 8]
    Call DebugStub_ComWriteEAX

	Call DebugStub_SendCoreDump
    PopAD 
  Pop EBP

DebugStub_SendSimpleLongNumber:
  Push EBP
  Mov EBP, ESP
  PushAD 

  Mov AL, DebugStub_Const_Ds2Vs_SimpleLongNumber
  Call DebugStub_ComWriteAL

  Mov EAX, DWORD [EBP + 8]
  Call DebugStub_ComWriteEAX
  Mov EAX, DWORD [EBP + 12]
  Call DebugStub_ComWriteEAX

  PopAD 
  Pop EBP

DebugStub_SendComplexNumber:
  Push EBP
  Mov EBP, ESP
  PushAD 

  Mov AL, DebugStub_Const_Ds2Vs_ComplexNumber
  Call DebugStub_ComWriteAL

  Mov EAX, DWORD [EBP + 8]
  Call DebugStub_ComWriteEAX

  PopAD 
  Pop EBP

DebugStub_SendComplexLongNumber:
  Push EBP
  Mov EBP, ESP
  PushAD 

  Mov AL, DebugStub_Const_Ds2Vs_ComplexLongNumber
  Call DebugStub_ComWriteAL

  Mov EAX, DWORD [EBP + 8]
  Call DebugStub_ComWriteEAX
  Mov EAX, DWORD [EBP + 12]
  Call DebugStub_ComWriteEAX

  PopAD 
  Pop EBP

DebugStub_SendPtr:
    Mov AL, DebugStub_Const_Ds2Vs_Pointer
    Call DebugStub_ComWriteAL

    Mov ESI, DWORD [EBP + 8]
    Call DebugStub_ComWrite32

DebugStub_SendStackCorruptionOccurred:
    Mov AL, DebugStub_Const_Ds2Vs_StackCorruptionOccurred
    Call DebugStub_ComWriteAL

    Mov ESI, DebugStub_CallerEIP
    Call DebugStub_ComWrite32

DebugStub_SendStackOverflowOccurred:
    Mov AL, DebugStub_Const_Ds2Vs_StackOverflowOccurred
    Call DebugStub_ComWriteAL

    Mov ESI, DebugStub_CallerEIP
    Call DebugStub_ComWrite32

DebugStub_SendInterruptOccurred:
	Push EAX

		Mov AL, DebugStub_Const_Ds2Vs_InterruptOccurred
		Call DebugStub_ComWriteAL

    Pop EAX
	Call DebugStub_ComWriteEAX

DebugStub_SendNullReferenceOccurred:
    Mov AL, DebugStub_Const_Ds2Vs_NullReferenceOccurred
    Call DebugStub_ComWriteAL

    Mov ESI, DebugStub_CallerEIP
    Call DebugStub_ComWrite32

DebugStub_SendMessageBox:
    Mov AL, DebugStub_Const_Ds2Vs_MessageBox
    Call DebugStub_ComWriteAL

    Mov ESI, EBP
    Add ESI, 0xC
    Mov ECX, DWORD [ESI]
    Call DebugStub_ComWrite16

    Mov ESI, DWORD [EBP + 8]
DebugStub_SendMessageBox_WriteChar:
    Call DebugStub_ComWrite8
    Dec ECX
    Inc ESI

DebugStub_SendCoreDump:
    Push EAX
    Push EBX
    Push ECX
    Push EDX
    Push EDI
    Push ESI
    Mov EAX, DebugStub_CallerEBP
    Push EAX
    Mov EAX, DebugStub_CallerEIP
    Push EAX
    Mov EAX, DebugStub_CallerESP
    Push EAX
    Mov ECX, 0x24
    Mov EAX, EBP
        Sub EAX, 0x4
        Push EAX
        Add ECX, 0x4
        Mov EAX, DWORD [EAX]
    DebugStub_SendCoreDump_Block1_End:

	Mov AL, DebugStub_Const_Ds2Vs_CoreDump
	Call DebugStub_ComWriteAL
    Mov EAX, ECX
    Call DebugStub_ComWriteAX
        Pop EAX
        Call DebugStub_ComWriteEAX
        Dec ECX
    DebugStub_SendCoreDump_Block2_End:
