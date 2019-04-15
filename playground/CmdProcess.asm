; namespace DebugStub

; Modifies: AL, DX (ComReadAL)
; Returns: AL
; function ProcessCommand {
DebugStub_ProcessCommand:
    ; ComReadAL()
    Call DebugStub_ComReadAL
    ; Some callers expect AL to be returned, so we preserve it
    ; in case any commands modify AL.
    ; We push EAX to keep stack aligned.
    ; +EAX
    Push EAX

    ; Noop has no data at all (see notes in client DebugConnector), so skip Command ID
    ; Noop also does not send ACK.
	; if AL = #Vs2Ds_Noop return

    ; Read Command ID
	; EAX = 0
	Mov EAX, 0x0
    ; ComReadAL()
    Call DebugStub_ComReadAL
    ; .CommandID = EAX
    Mov DWORD [DebugStub_CommandID], EAX

    ; Get AL back so we can compare it, but also leave it for later
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
		; Ack command for a break must be done first
		; Otherwise we Break then ProcessCommands and get stuck because we don't send this Ack until execution continues
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
    ; Restore AL for callers who check the command and do
    ; further processing, or for commands not handled by this function.
    ; -EAX
    Pop EAX
; }

; function AckCommand {
DebugStub_AckCommand:
    ; We acknowledge receipt of the command AND the processing of it.
    ; -In the past the ACK only acknowledged receipt.
    ; We have to do this because sometimes callers do more processing.
    ; We ACK even ones we dont process here, but do not ACK Noop.
    ; The buffers should be ok because more wont be sent till after our NACK
    ; is received.
    ; Right now our max cmd size is 2 (Cmd + Cmd ID) + 5 (Data) = 7.
    ; UART buffer is 16.
    ; We may need to revisit this in the future to ack not commands, but data chunks
    ; and move them to a buffer.
    ; The buffer problem exists only to inbound data, not outbound data (relative to DebugStub).

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

    ; See if batch is complete
    ; Loop and wait
	; Vs2Ds.BatchEnd
	; if AL != 8 goto Begin

    ; AckCommand()
    Call DebugStub_AckCommand
; }
