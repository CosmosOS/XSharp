
DebugStub_CallerEBP dd 0
DebugStub_CallerEIP dd 0
DebugStub_CallerESP dd 0

DebugStub_TraceMode dd 0
DebugStub_DebugStatus dd 0
DebugStub_PushAllPtr dd 0
DebugStub_DebugBreakOnNextTrace dd 0
DebugStub_BreakEBP dd 0
DebugStub_CommandID dd 0

DebugStub_BreakOnAddress:
	PushAD 
  Call DebugStub_ComReadEAX
  Mov ECX, EAX

  Mov EAX, 0x0
  Call DebugStub_ComReadAL

	Push EAX

	Mov EBX, DebugStub_DebugBPs
  Shl EAX, 0x2
  Add EBX, EAX


		Mov EDI, DWORD [EBX]
		Mov AL, 0x90
		Mov BYTE [EDI], AL

	DebugStub_BreakOnAddress_Block1_End:

    Mov DWORD [EBX], ECX
	Mov EDI, DWORD [EBX]
	Mov AL, 0xCC
	Mov BYTE [EDI], AL

DebugStub_BreakOnAddress_DontSetBP:

	Pop EAX


	Mov ECX, 0x100
DebugStub_BreakOnAddress_FindBPLoop:
	Dec ECX

	Mov EBX, DebugStub_DebugBPs
	Mov EAX, ECX
	Shl EAX, 0x2
	Add EBX, EAX

	Mov EAX, DWORD [EBX]

		Inc ECX
		Mov DWORD [DebugStub_MaxBPId], ECX
	DebugStub_BreakOnAddress_Block2_End:
	DebugStub_BreakOnAddress_Block3_End:

DebugStub_BreakOnAddress_FindBPLoopExit:
	Mov DWORD [DebugStub_MaxBPId], 0x0

DebugStub_BreakOnAddress_Continue:
DebugStub_BreakOnAddress_Exit:
	PopAD 

DebugStub_SetINT3:
	PushAD 

    Call DebugStub_ComReadEAX
    Mov EDI, EAX
	Mov AL, 0xCC
	Mov BYTE [EDI], AL

DebugStub_SetINT3_Exit:
	PopAD 
DebugStub_ClearINT3:
	PushAD 

    Call DebugStub_ComReadEAX
    Mov EDI, EAX
	Mov AL, 0x90
	Mov BYTE [EDI], AL

DebugStub_ClearINT3_Exit:
	PopAD 

DebugStub_Executing:

	 MOV EAX, DR6

	   MOV DR6, EAX

	   Call DebugStub_ResetINT1_TrapFLAG

	   Call DebugStub_Break
	 DebugStub_Executing_Block1_End:

    Mov EAX, DWORD [DebugStub_CallerEIP]
		Call DebugStub_DoAsmBreak
	DebugStub_Executing_Block2_End:


    Mov EAX, DWORD [DebugStub_MaxBPId]
	DebugStub_Executing_Block3_End:

	Mov EAX, DWORD [DebugStub_CallerEIP]
    Mov EDI, DebugStub_DebugBPs
    Mov ECX, DWORD [DebugStub_MaxBPId]
	repne scasd
		Call DebugStub_Break
	DebugStub_Executing_Block4_End:
DebugStub_Executing_SkipBPScan:


		Call DebugStub_Break
	DebugStub_Executing_Block5_End:

	Mov EAX, DWORD [DebugStub_CallerEBP]

			Call DebugStub_Break
		DebugStub_Executing_Block7_End:
	DebugStub_Executing_Block6_End:

			Call DebugStub_Break
		DebugStub_Executing_Block9_End:
	DebugStub_Executing_Block8_End:

DebugStub_Executing_Normal:
		Call DebugStub_SendTrace
	DebugStub_Executing_Block10_End:

DebugStub_Executing_CheckForCmd:
	  Mov DX, 0x5
    Call DebugStub_ReadRegister
    Test AL, 0x1
		Call DebugStub_ProcessCommand
	DebugStub_Executing_Block11_End:

DebugStub_Break:
    Mov [DebugStub_DebugBreakOnNextTrace], DebugStub_Const_StepTrigger_None
    Mov DWORD [DebugStub_BreakEBP], 0x0
    Mov [DebugStub_DebugStatus], DebugStub_Const_Status_Break
    Call DebugStub_SendTrace

DebugStub_Break_WaitCmd:
    Call DebugStub_ProcessCommand



		Call DebugStub_SetINT1_TrapFLAG
	DebugStub_Break_Block1_End:

        Call DebugStub_SetAsmBreak
	    Call DebugStub_AckCommand
	DebugStub_Break_Block2_End:

        Mov [DebugStub_DebugBreakOnNextTrace], DebugStub_Const_StepTrigger_Into
        Mov DWORD [DebugStub_BreakEBP], EAX
	DebugStub_Break_Block3_End:

        Mov [DebugStub_DebugBreakOnNextTrace], DebugStub_Const_StepTrigger_Over
        Mov EAX, DWORD [DebugStub_CallerEBP]
        Mov DWORD [DebugStub_BreakEBP], EAX
	DebugStub_Break_Block4_End:

        Mov [DebugStub_DebugBreakOnNextTrace], DebugStub_Const_StepTrigger_Out
        Mov EAX, DWORD [DebugStub_CallerEBP]
        Mov DWORD [DebugStub_BreakEBP], EAX
	DebugStub_Break_Block5_End:


DebugStub_Break_Done:
    Call DebugStub_AckCommand
    Mov [DebugStub_DebugStatus], DebugStub_Const_Status_Run

