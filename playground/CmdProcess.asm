
DebugStub_ProcessCommand:
    Call DebugStub_ComReadAL
    Push EAX


	Mov EAX, 0x0
    Call DebugStub_ComReadAL
    Mov DWORD [DebugStub_CommandID], EAX

    Mov EAX, DWORD [ESP]

		Call DebugStub_TraceOff
		Call DebugStub_AckCommand
		Ret 
	DebugStub_ProcessCommand_Block1_End:
		Call DebugStub_TraceOn
		Call DebugStub_AckCommand
		Ret 
	DebugStub_ProcessCommand_Block2_End:
		Call DebugStub_AckCommand
		Call DebugStub_Break
		Ret 
	DebugStub_ProcessCommand_Block3_End:
		Call DebugStub_BreakOnAddress
		Call DebugStub_AckCommand
		Ret 
	DebugStub_ProcessCommand_Block4_End:
		Call DebugStub_SendMethodContext
		Call DebugStub_AckCommand
		Ret 
	DebugStub_ProcessCommand_Block5_End:
		Call DebugStub_SendMemory
		Call DebugStub_AckCommand
		Ret 
	DebugStub_ProcessCommand_Block6_End:
		Call DebugStub_SendRegisters
		Call DebugStub_AckCommand
		Ret 
	DebugStub_ProcessCommand_Block7_End:
		Call DebugStub_SendFrame
		Call DebugStub_AckCommand
		Ret 
	DebugStub_ProcessCommand_Block8_End:
		Call DebugStub_SendStack
		Call DebugStub_AckCommand
		Ret 
	DebugStub_ProcessCommand_Block9_End:
		Call DebugStub_Ping
		Call DebugStub_AckCommand
		Ret 
	DebugStub_ProcessCommand_Block10_End:
		Call DebugStub_SetINT3
		Call DebugStub_AckCommand
		Ret 
	DebugStub_ProcessCommand_Block11_End:
		Call DebugStub_ClearINT3
		Call DebugStub_AckCommand
		Ret 
	DebugStub_ProcessCommand_Block12_End:


DebugStub_ProcessCommand_Exit:
    Pop EAX

DebugStub_AckCommand:

	Mov AL, DebugStub_Const_Ds2Vs_CmdCompleted
    Call DebugStub_ComWriteAL
    
    Mov EAX, DWORD [DebugStub_CommandID]
    Call DebugStub_ComWriteAL

DebugStub_ProcessCommandBatch:
DebugStub_ProcessCommandBatch_Begin:
    Call DebugStub_ProcessCommand


    Call DebugStub_AckCommand
