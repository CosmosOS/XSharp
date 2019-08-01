; namespace DebugStub

; function ProcessCommand {
DebugStub_ProcessCommand:
    ; ComReadAL()
    Call DebugStub_ComReadAL
    ; +EAX
    Push EAX

	; if AL = #Vs2Ds_Noop return

	; EAX = 0
	Mov EAX, 0x0
    ; ComReadAL()
    Call DebugStub_ComReadAL
    ; .CommandID = EAX
    Mov DWORD [DebugStub_CommandID], EAX

    ; EAX = [ESP]
    Mov EAX, DWORD [ESP]

	; if AL = #Vs2Ds_TraceOff {
		; TraceOff()
		Call DebugStub_TraceOff
		; AckCommand()
		Call DebugStub_AckCommand
		; return
		Ret 
	; }
	; if AL = #Vs2Ds_TraceOn {
		; TraceOn()
		Call DebugStub_TraceOn
		; AckCommand()
		Call DebugStub_AckCommand
		; return
		Ret 
	; }
	; if AL = #Vs2Ds_Break {
		; AckCommand()
		Call DebugStub_AckCommand
		; Break()
		Call DebugStub_Break
		; return
		Ret 
	; }
	; if AL = #Vs2Ds_BreakOnAddress {
		; BreakOnAddress()
		Call DebugStub_BreakOnAddress
		; AckCommand()
		Call DebugStub_AckCommand
		; return
		Ret 
	; }
	; if AL = #Vs2Ds_SendMethodContext {
		; SendMethodContext()
		Call DebugStub_SendMethodContext
		; AckCommand()
		Call DebugStub_AckCommand
		; return
		Ret 
	; }
	; if AL = #Vs2Ds_SendMemory {
		; SendMemory()
		Call DebugStub_SendMemory
		; AckCommand()
		Call DebugStub_AckCommand
		; return
		Ret 
	; }
	; if AL = #Vs2Ds_SendRegisters {
		; SendRegisters()
		Call DebugStub_SendRegisters
		; AckCommand()
		Call DebugStub_AckCommand
		; return
		Ret 
	; }
	; if AL = #Vs2Ds_SendFrame {
		; SendFrame()
		Call DebugStub_SendFrame
		; AckCommand()
		Call DebugStub_AckCommand
		; return
		Ret 
	; }
	; if AL = #Vs2Ds_SendStack {
		; SendStack()
		Call DebugStub_SendStack
		; AckCommand()
		Call DebugStub_AckCommand
		; return
		Ret 
	; }
	; if AL = #Vs2Ds_Ping {
		; Ping()
		Call DebugStub_Ping
		; AckCommand()
		Call DebugStub_AckCommand
		; return
		Ret 
	; }
	; if AL = #Vs2Ds_SetINT3 {
		; SetINT3()
		Call DebugStub_SetINT3
		; AckCommand()
		Call DebugStub_AckCommand
		; return
		Ret 
	; }
	; if AL = #Vs2Ds_ClearINT3 {
		; ClearINT3()
		Call DebugStub_ClearINT3
		; AckCommand()
		Call DebugStub_AckCommand
		; return
		Ret 
	; }


; Exit:
DebugStub_Exit:
    ; -EAX
    Pop EAX
; }

; function AckCommand {
DebugStub_AckCommand:

	; AL = #Ds2Vs_CmdCompleted
	Mov AL, DebugStub_Const_Ds2Vs_CmdCompleted
    ; ComWriteAL()
    Call DebugStub_ComWriteAL
    
    ; EAX = .CommandID
    Mov EAX, DWORD [DebugStub_CommandID]
    ; ComWriteAL()
    Call DebugStub_ComWriteAL
; }

; function ProcessCommandBatch {
DebugStub_ProcessCommandBatch:
; Begin:
DebugStub_Begin:
    ; ProcessCommand()
    Call DebugStub_ProcessCommand

	; if AL != 8 goto Begin

    ; AckCommand()
    Call DebugStub_AckCommand
; }
