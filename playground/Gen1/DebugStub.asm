; Generated at 4/14/2019 1:59:47 AM

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
Pushad
Call DebugStub_ComReadEAX
mov ECX, EAX
mov EAX, 0x0
Call DebugStub_ComReadAL
push dword EAX
mov EBX, DebugStub_DebugBPs
shl EAX, 0x2
add EBX, EAX
cmp ECX, 0x0
JNE near DebugStub_BreakOnAddress_Block1_End
mov EDI, dword [EBX]
mov AL, 0x90
mov byte [EDI], AL
Jmp DebugStub_BreakOnAddress_DontSetBP

DebugStub_BreakOnAddress_Block1_End:
mov dword [EBX], ECX
mov EDI, dword [EBX]
mov AL, 0xCC
mov byte [EDI], AL

DebugStub_BreakOnAddress_DontSetBP:
pop dword EAX
mov ECX, 0x100

DebugStub_BreakOnAddress_FindBPLoop:
dec dword ECX
mov EBX, DebugStub_DebugBPs
mov EAX, ECX
shl EAX, 0x2
add EBX, EAX
mov EAX, dword [EBX]
cmp EAX, 0x0
JE near DebugStub_BreakOnAddress_Block2_End
inc dword ECX
mov dword [DebugStub_MaxBPId], ECX
Jmp DebugStub_BreakOnAddress_Continue

DebugStub_BreakOnAddress_Block2_End:
cmp ECX, 0x0
JNE near DebugStub_BreakOnAddress_Block3_End
Jmp DebugStub_BreakOnAddress_FindBPLoopExit

DebugStub_BreakOnAddress_Block3_End:
Jmp DebugStub_BreakOnAddress_FindBPLoop

DebugStub_BreakOnAddress_FindBPLoopExit:
mov dword [DebugStub_MaxBPId], 0x0

DebugStub_BreakOnAddress_Continue:

DebugStub_BreakOnAddress_Exit:
Popad
mov dword [INTs_LastKnownAddress], DebugStub_BreakOnAddress_Exit
Ret


DebugStub_SetINT3:
Pushad
Call DebugStub_ComReadEAX
mov EDI, EAX
mov AL, 0xCC
mov byte [EDI], AL

DebugStub_SetINT3_Exit:
Popad
mov dword [INTs_LastKnownAddress], DebugStub_SetINT3_Exit
Ret


DebugStub_ClearINT3:
Pushad
Call DebugStub_ComReadEAX
mov EDI, EAX
mov AL, 0x90
mov byte [EDI], AL

DebugStub_ClearINT3_Exit:
Popad
mov dword [INTs_LastKnownAddress], DebugStub_ClearINT3_Exit
Ret


DebugStub_Executing:
MOV EAX, DR6
and EAX, 0x4000
cmp EAX, 0x4000
JNE near DebugStub_Executing_Block1_End
and EAX, 0xBFFF
MOV DR6, EAX
Call DebugStub_ResetINT1_TrapFLAG
Call DebugStub_Break
Jmp DebugStub_Executing_Normal

DebugStub_Executing_Block1_End:
mov EAX, dword [DebugStub_CallerEIP]
cmp EAX, dword [DebugStub_AsmBreakEIP]
JNE near DebugStub_Executing_Block2_End
Call DebugStub_DoAsmBreak
Jmp DebugStub_Executing_Normal

DebugStub_Executing_Block2_End:
mov EAX, dword [DebugStub_MaxBPId]
cmp EAX, 0x0
JNE near DebugStub_Executing_Block3_End
Jmp DebugStub_Executing_SkipBPScan

DebugStub_Executing_Block3_End:
mov EAX, dword [DebugStub_CallerEIP]
mov EDI, DebugStub_DebugBPs
mov ECX, dword [DebugStub_MaxBPId]
repne scasd
JNE near DebugStub_Executing_Block4_End
Call DebugStub_Break
Jmp DebugStub_Executing_Normal

DebugStub_Executing_Block4_End:

DebugStub_Executing_SkipBPScan:
cmp dword [DebugStub_DebugBreakOnNextTrace], DebugStub_Const_StepTrigger_Into
JNE near DebugStub_Executing_Block5_End
Call DebugStub_Break
Jmp DebugStub_Executing_Normal

DebugStub_Executing_Block5_End:
mov EAX, dword [DebugStub_CallerEBP]
cmp dword [DebugStub_DebugBreakOnNextTrace], DebugStub_Const_StepTrigger_Over
JNE near DebugStub_Executing_Block6_End
cmp EAX, dword [DebugStub_BreakEBP]
JB near DebugStub_Executing_Block7_End
Call DebugStub_Break

DebugStub_Executing_Block7_End:
Jmp DebugStub_Executing_Normal

DebugStub_Executing_Block6_End:
cmp dword [DebugStub_DebugBreakOnNextTrace], DebugStub_Const_StepTrigger_Out
JNE near DebugStub_Executing_Block8_End
cmp EAX, dword [DebugStub_BreakEBP]
JBE near DebugStub_Executing_Block9_End
Call DebugStub_Break

DebugStub_Executing_Block9_End:
Jmp DebugStub_Executing_Normal

DebugStub_Executing_Block8_End:

DebugStub_Executing_Normal:
cmp dword [DebugStub_TraceMode], DebugStub_Const_Tracing_On
JNE near DebugStub_Executing_Block10_End
Call DebugStub_SendTrace

DebugStub_Executing_Block10_End:

DebugStub_Executing_CheckForCmd:
mov DX, 0x5
Call DebugStub_ReadRegister
test AL, 0x1
JE near DebugStub_Executing_Block11_End
Call DebugStub_ProcessCommand
Jmp DebugStub_Executing_CheckForCmd

DebugStub_Executing_Block11_End:

DebugStub_Executing_Exit:
mov dword [INTs_LastKnownAddress], DebugStub_Executing_Exit
Ret


DebugStub_Break:
mov dword [DebugStub_DebugBreakOnNextTrace], DebugStub_Const_StepTrigger_None
mov dword [DebugStub_BreakEBP], 0x0
mov dword [DebugStub_DebugStatus], DebugStub_Const_Status_Break
Call DebugStub_SendTrace

DebugStub_Break_WaitCmd:
Call DebugStub_ProcessCommand
cmp AL, DebugStub_Const_Vs2Ds_Continue
JE near DebugStub_Break_Done
cmp AL, DebugStub_Const_Vs2Ds_AsmStepInto
JNE near DebugStub_Break_Block1_End
Call DebugStub_SetINT1_TrapFLAG
Jmp DebugStub_Break_Done

DebugStub_Break_Block1_End:
cmp AL, DebugStub_Const_Vs2Ds_SetAsmBreak
JNE near DebugStub_Break_Block2_End
Call DebugStub_SetAsmBreak
Call DebugStub_AckCommand
Jmp DebugStub_Break_WaitCmd

DebugStub_Break_Block2_End:
cmp AL, DebugStub_Const_Vs2Ds_StepInto
JNE near DebugStub_Break_Block3_End
mov dword [DebugStub_DebugBreakOnNextTrace], DebugStub_Const_StepTrigger_Into
mov dword [DebugStub_BreakEBP], EAX
Jmp DebugStub_Break_Done

DebugStub_Break_Block3_End:
cmp AL, DebugStub_Const_Vs2Ds_StepOver
JNE near DebugStub_Break_Block4_End
mov dword [DebugStub_DebugBreakOnNextTrace], DebugStub_Const_StepTrigger_Over
mov EAX, dword [DebugStub_CallerEBP]
mov dword [DebugStub_BreakEBP], EAX
Jmp DebugStub_Break_Done

DebugStub_Break_Block4_End:
cmp AL, DebugStub_Const_Vs2Ds_StepOut
JNE near DebugStub_Break_Block5_End
mov dword [DebugStub_DebugBreakOnNextTrace], DebugStub_Const_StepTrigger_Out
mov EAX, dword [DebugStub_CallerEBP]
mov dword [DebugStub_BreakEBP], EAX
Jmp DebugStub_Break_Done

DebugStub_Break_Block5_End:
Jmp DebugStub_Break_WaitCmd

DebugStub_Break_Done:
Call DebugStub_AckCommand
mov dword [DebugStub_DebugStatus], DebugStub_Const_Status_Run

DebugStub_Break_Exit:
mov dword [INTs_LastKnownAddress], DebugStub_Break_Exit
Ret

