; namespace DebugStub

; Modifies: AL, DX (ComReadAL)
; Returns: AL
; function ProcessCommand {
    ; ComReadAL()
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
    ; .CommandID = EAX
    Mov DWORD [DebugStub_Var_CommandID], EAX

    ; Get AL back so we can compare it, but also leave it for later
    ; EAX = [ESP]
    Mov EAX, DWORD [ESP]

	; if AL = #Vs2Ds_TraceOff {
		; TraceOff()
		; AckCommand()
		; return
		Ret 
	; }
	; if AL = #Vs2Ds_TraceOn {
		; TraceOn()
		; AckCommand()
		; return
		Ret 
	; }
	; if AL = #Vs2Ds_Break {
		; Ack command for a break must be done first
		; Otherwise we Break then ProcessCommands and get stuck because we don't send this Ack until execution continues
		; AckCommand()
		; Break()
		; return
		Ret 
	; }
	; if AL = #Vs2Ds_BreakOnAddress {
		; BreakOnAddress()
		; AckCommand()
		; return
		Ret 
	; }
	; if AL = #Vs2Ds_SendMethodContext {
		; SendMethodContext()
		; AckCommand()
		; return
		Ret 
	; }
	; if AL = #Vs2Ds_SendMemory {
		; SendMemory()
		; AckCommand()
		; return
		Ret 
	; }
	; if AL = #Vs2Ds_SendRegisters {
		; SendRegisters()
		; AckCommand()
		; return
		Ret 
	; }
	; if AL = #Vs2Ds_SendFrame {
		; SendFrame()
		; AckCommand()
		; return
		Ret 
	; }
	; if AL = #Vs2Ds_SendStack {
		; SendStack()
		; AckCommand()
		; return
		Ret 
	; }
	; if AL = #Vs2Ds_Ping {
		; Ping()
		; AckCommand()
		; return
		Ret 
	; }
	; if AL = #Vs2Ds_SetINT3 {
		; SetINT3()
		; AckCommand()
		; return
		Ret 
	; }
	; if AL = #Vs2Ds_ClearINT3 {
		; ClearINT3()
		; AckCommand()
		; return
		Ret 
	; }


; Exit:
    ; Restore AL for callers who check the command and do
    ; further processing, or for commands not handled by this function.
    ; -EAX
    Pop EAX
; }

; function AckCommand {
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
    
    ; EAX = .CommandID
    Mov EAX, DWORD [DebugStub_Var_CommandID]
    ; ComWriteAL()
; }

; function ProcessCommandBatch {
; Begin:
    ; ProcessCommand()

    ; See if batch is complete
    ; Loop and wait
	; Vs2Ds.BatchEnd
	; if AL != 8 goto Begin

    ; AckCommand()
; }
