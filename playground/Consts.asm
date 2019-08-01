; namespace DebugStub

; const Signature = $19740807
DebugStub_Const_Signature equ 427034631

; const Tracing_Off = 0
DebugStub_Const_Tracing_Off equ 0
; const Tracing_On = 1
DebugStub_Const_Tracing_On equ 1

; const Status_Run = 0
DebugStub_Const_Status_Run equ 0
; const Status_Break = 1
DebugStub_Const_Status_Break equ 1

; const StepTrigger_None = 0
DebugStub_Const_StepTrigger_None equ 0
; const StepTrigger_Into = 1
DebugStub_Const_StepTrigger_Into equ 1
; const StepTrigger_Over = 2
DebugStub_Const_StepTrigger_Over equ 2
; const StepTrigger_Out = 3
DebugStub_Const_StepTrigger_Out equ 3

; const Vs2Ds_Noop = 0
DebugStub_Const_Vs2Ds_Noop equ 0
; const Vs2Ds_TraceOff = 1
DebugStub_Const_Vs2Ds_TraceOff equ 1
; const Vs2Ds_TraceOn = 2
DebugStub_Const_Vs2Ds_TraceOn equ 2
; const Vs2Ds_Break = 3
DebugStub_Const_Vs2Ds_Break equ 3
; const Vs2Ds_Continue = 4
DebugStub_Const_Vs2Ds_Continue equ 4
; const Vs2Ds_BreakOnAddress = 6
DebugStub_Const_Vs2Ds_BreakOnAddress equ 6
; const Vs2Ds_BatchBegin = 7
DebugStub_Const_Vs2Ds_BatchBegin equ 7
; const Vs2Ds_BatchEnd = 8
DebugStub_Const_Vs2Ds_BatchEnd equ 8
; const Vs2Ds_StepInto = 5
DebugStub_Const_Vs2Ds_StepInto equ 5
; const Vs2Ds_StepOver = 11
DebugStub_Const_Vs2Ds_StepOver equ 11
; const Vs2Ds_StepOut = 12
DebugStub_Const_Vs2Ds_StepOut equ 12
; const Vs2Ds_SendMethodContext = 9
DebugStub_Const_Vs2Ds_SendMethodContext equ 9
; const Vs2Ds_SendMemory = 10
DebugStub_Const_Vs2Ds_SendMemory equ 10
; const Vs2Ds_SendRegisters = 13
DebugStub_Const_Vs2Ds_SendRegisters equ 13
; const Vs2Ds_SendFrame = 14
DebugStub_Const_Vs2Ds_SendFrame equ 14
; const Vs2Ds_SendStack = 15
DebugStub_Const_Vs2Ds_SendStack equ 15
; const Vs2Ds_SetAsmBreak = 16
DebugStub_Const_Vs2Ds_SetAsmBreak equ 16
; const Vs2Ds_Ping = 17
DebugStub_Const_Vs2Ds_Ping equ 17
; const Vs2Ds_AsmStepInto = 18
DebugStub_Const_Vs2Ds_AsmStepInto equ 18
; const Vs2Ds_SetINT3 = 19
DebugStub_Const_Vs2Ds_SetINT3 equ 19
; const Vs2Ds_ClearINT3 = 20
DebugStub_Const_Vs2Ds_ClearINT3 equ 20
; const Vs2Ds_Max = 21
DebugStub_Const_Vs2Ds_Max equ 21

; const Ds2Vs_Noop = 0
DebugStub_Const_Ds2Vs_Noop equ 0
; const Ds2Vs_TracePoint = 1
DebugStub_Const_Ds2Vs_TracePoint equ 1
; const Ds2Vs_Message = 192
DebugStub_Const_Ds2Vs_Message equ 192
; const Ds2Vs_BreakPoint = 3
DebugStub_Const_Ds2Vs_BreakPoint equ 3
; const Ds2Vs_Error = 4
DebugStub_Const_Ds2Vs_Error equ 4
; const Ds2Vs_Pointer = 5
DebugStub_Const_Ds2Vs_Pointer equ 5
; const Ds2Vs_Started = 6
DebugStub_Const_Ds2Vs_Started equ 6
; const Ds2Vs_MethodContext = 7
DebugStub_Const_Ds2Vs_MethodContext equ 7
; const Ds2Vs_MemoryData = 8
DebugStub_Const_Ds2Vs_MemoryData equ 8
; const Ds2Vs_CmdCompleted = 9
DebugStub_Const_Ds2Vs_CmdCompleted equ 9
; const Ds2Vs_Registers = 10
DebugStub_Const_Ds2Vs_Registers equ 10
; const Ds2Vs_Frame = 11
DebugStub_Const_Ds2Vs_Frame equ 11
; const Ds2Vs_Stack = 12
DebugStub_Const_Ds2Vs_Stack equ 12
; const Ds2Vs_Pong = 13
DebugStub_Const_Ds2Vs_Pong equ 13
; const Ds2Vs_BreakPointAsm = 14
DebugStub_Const_Ds2Vs_BreakPointAsm equ 14
; const Ds2Vs_StackCorruptionOccurred = 15
DebugStub_Const_Ds2Vs_StackCorruptionOccurred equ 15
; const Ds2Vs_MessageBox = 16
DebugStub_Const_Ds2Vs_MessageBox equ 16
; const Ds2Vs_NullReferenceOccurred = 17
DebugStub_Const_Ds2Vs_NullReferenceOccurred equ 17
; const Ds2Vs_SimpleNumber = 18
DebugStub_Const_Ds2Vs_SimpleNumber equ 18
; const Ds2Vs_SimpleLongNumber = 19
DebugStub_Const_Ds2Vs_SimpleLongNumber equ 19
; const Ds2Vs_ComplexNumber = 20
DebugStub_Const_Ds2Vs_ComplexNumber equ 20
; const Ds2Vs_ComplexLongNumber = 21
DebugStub_Const_Ds2Vs_ComplexLongNumber equ 21
; const Ds2Vs_StackOverflowOccurred = 22
DebugStub_Const_Ds2Vs_StackOverflowOccurred equ 22
; const Ds2Vs_InterruptOccurred = 23
DebugStub_Const_Ds2Vs_InterruptOccurred equ 23
; const Ds2Vs_CoreDump = 24
DebugStub_Const_Ds2Vs_CoreDump equ 24
; const Ds2Vs_KernelPanic = 25
DebugStub_Const_Ds2Vs_KernelPanic equ 25
