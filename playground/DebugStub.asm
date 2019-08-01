; namespace DebugStub

; var CallerEBP
DebugStub_CallerEBP dd 0
; var CallerEIP
DebugStub_CallerEIP dd 0
; var CallerESP
DebugStub_CallerESP dd 0

; var TraceMode
DebugStub_TraceMode dd 0
; var DebugStatus
DebugStub_DebugStatus dd 0
; var PushAllPtr
DebugStub_PushAllPtr dd 0
; var DebugBreakOnNextTrace
DebugStub_DebugBreakOnNextTrace dd 0
; var BreakEBP
DebugStub_BreakEBP dd 0
; var CommandID
DebugStub_CommandID dd 0

; function BreakOnAddress {
DebugStub_BreakOnAddress:
	; +All
	PushAD 
  ; ComReadEAX()
  Call DebugStub_ComReadEAX
  ; ECX = EAX
  Mov ECX, EAX

  ; EAX = 0
  Mov EAX, 0x0
  ; ComReadAL()
  Call DebugStub_ComReadAL

	; +EAX
	Push EAX

	; EBX = @.DebugBPs
	Mov EBX, DebugStub_DebugBPs
  ; EAX << 2
  Shl EAX, 0x2
  ; EBX += EAX
  Add EBX, EAX

	; if ECX = 0 {

		; EDI = [EBX]
		Mov EDI, DWORD [EBX]
		; AL = $90
		Mov AL, 0x90
		; [EDI] = AL
		Mov BYTE [EDI], AL

		; goto DontSetBP
	; }

    ; [EBX] = ECX
    Mov DWORD [EBX], ECX
	; EDI = [EBX]
	Mov EDI, DWORD [EBX]
	; AL = $CC
	Mov AL, 0xCC
	; [EDI] = AL
	Mov BYTE [EDI], AL

; DontSetBP:
DebugStub_DontSetBP:

	; -EAX
	Pop EAX


	; ECX = 256
	Mov ECX, 0x100
; FindBPLoop:
DebugStub_FindBPLoop:
	; ECX--
	Dec ECX

	; EBX = @.DebugBPs
	Mov EBX, DebugStub_DebugBPs
	; EAX = ECX
	Mov EAX, ECX
	; EAX << 2
	Shl EAX, 0x2
	; EBX += EAX
	Add EBX, EAX

	; EAX = [EBX]
	Mov EAX, DWORD [EBX]
	; if EAX != 0 {

		; ECX++
		Inc ECX
		; .MaxBPId = ECX
		Mov DWORD [DebugStub_MaxBPId], ECX
		; goto Continue
	; }
	; if ECX = 0 {
		; goto FindBPLoopExit
	; }
	; goto FindBPLoop

; FindBPLoopExit:
DebugStub_FindBPLoopExit:
	; .MaxBPId = 0
	Mov DWORD [DebugStub_MaxBPId], 0x0

; Continue:
DebugStub_Continue:
; Exit:
DebugStub_Exit:
	; -All
	PopAD 
; }

; function SetINT3 {
DebugStub_SetINT3:
	; +All
	PushAD 

    ; ComReadEAX()
    Call DebugStub_ComReadEAX
    ; EDI = EAX
    Mov EDI, EAX
	; AL = $CC
	Mov AL, 0xCC
	; [EDI] = AL
	Mov BYTE [EDI], AL

; Exit:
DebugStub_Exit:
	; -All
	PopAD 
; }
; function ClearINT3 {
DebugStub_ClearINT3:
	; +All
	PushAD 

    ; ComReadEAX()
    Call DebugStub_ComReadEAX
    ; EDI = EAX
    Mov EDI, EAX
	; AL = $90
	Mov AL, 0x90
	; [EDI] = AL
	Mov BYTE [EDI], AL

; Exit:
DebugStub_Exit:
	; -All
	PopAD 
; }

; function Executing {
DebugStub_Executing:

	 ; //! MOV EAX, DR6
	 MOV EAX, DR6
	 ; EAX & $4000
	 ; if EAX = $4000 {

	   ; EAX & $BFFF
	   ; //! MOV DR6, EAX
	   MOV DR6, EAX

	   ; ResetINT1_TrapFLAG()
	   Call DebugStub_ResetINT1_TrapFLAG

	   ; Break()
	   Call DebugStub_Break
	   ; goto Normal
	 ; }

    ; EAX = .CallerEIP
    Mov EAX, DWORD [DebugStub_CallerEIP]
	; if EAX = .AsmBreakEIP {
		; DoAsmBreak()
		Call DebugStub_DoAsmBreak
  		; goto Normal
	; }


    ; EAX = .MaxBPId
    Mov EAX, DWORD [DebugStub_MaxBPId]
	; if EAX = 0 {
		; goto SkipBPScan
	; }

	; EAX = .CallerEIP
	Mov EAX, DWORD [DebugStub_CallerEIP]
    ; EDI = @.DebugBPs
    Mov EDI, DebugStub_DebugBPs
    ; ECX = .MaxBPId
    Mov ECX, DWORD [DebugStub_MaxBPId]
	; //! repne scasd
	repne scasd
	; if = {
		; Break()
		Call DebugStub_Break
		; goto Normal
	; }
; SkipBPScan:
DebugStub_SkipBPScan:


    ; if dword .DebugBreakOnNextTrace = #StepTrigger_Into {
		; Break()
		Call DebugStub_Break
		; goto Normal
	; }

	; EAX = .CallerEBP
	Mov EAX, DWORD [DebugStub_CallerEBP]

    ; if dword .DebugBreakOnNextTrace = #StepTrigger_Over {
		; if EAX >= .BreakEBP {
			; Break()
			Call DebugStub_Break
		; }
		; goto Normal
	; }

    ; if dword .DebugBreakOnNextTrace = #StepTrigger_Out {
		; if EAX > .BreakEBP {
			; Break()
			Call DebugStub_Break
		; }
		; goto Normal
	; }

; Normal:
DebugStub_Normal:
	; if dword .TraceMode = #Tracing_On {
		; SendTrace()
		Call DebugStub_SendTrace
	; }

; CheckForCmd:
DebugStub_CheckForCmd:
	  ; DX = 5
	  Mov DX, 0x5
    ; ReadRegister()
    Call DebugStub_ReadRegister
    ; AL test 1
    Test AL, 0x1
	; if !0 {
		; ProcessCommand()
		Call DebugStub_ProcessCommand
		; goto CheckForCmd
	; }
; }

; function Break {
DebugStub_Break:
    ; .DebugBreakOnNextTrace = #StepTrigger_None
    Mov [DebugStub_DebugBreakOnNextTrace], DebugStub_Const_StepTrigger_None
    ; .BreakEBP = 0
    Mov DWORD [DebugStub_BreakEBP], 0x0
    ; .DebugStatus = #Status_Break
    Mov [DebugStub_DebugStatus], DebugStub_Const_Status_Break
    ; SendTrace()
    Call DebugStub_SendTrace

; WaitCmd:
DebugStub_WaitCmd:
    ; ProcessCommand()
    Call DebugStub_ProcessCommand


    ; if AL = #Vs2Ds_Continue goto Done

	; if AL = #Vs2Ds_AsmStepInto {
		; SetINT1_TrapFLAG()
		Call DebugStub_SetINT1_TrapFLAG
		; goto Done
	; }

    ; if AL = #Vs2Ds_SetAsmBreak {
        ; SetAsmBreak()
        Call DebugStub_SetAsmBreak
	    ; AckCommand()
	    Call DebugStub_AckCommand
	    ; goto WaitCmd
	; }

    ; if AL = #Vs2Ds_StepInto {
        ; .DebugBreakOnNextTrace = #StepTrigger_Into
        Mov [DebugStub_DebugBreakOnNextTrace], DebugStub_Const_StepTrigger_Into
        ; .BreakEBP = EAX
        Mov DWORD [DebugStub_BreakEBP], EAX
	    ; goto Done
	; }

    ; if AL = #Vs2Ds_StepOver {
        ; .DebugBreakOnNextTrace = #StepTrigger_Over
        Mov [DebugStub_DebugBreakOnNextTrace], DebugStub_Const_StepTrigger_Over
        ; EAX = .CallerEBP
        Mov EAX, DWORD [DebugStub_CallerEBP]
        ; .BreakEBP = EAX
        Mov DWORD [DebugStub_BreakEBP], EAX
	    ; goto Done
	; }

    ; if AL = #Vs2Ds_StepOut {
        ; .DebugBreakOnNextTrace = #StepTrigger_Out
        Mov [DebugStub_DebugBreakOnNextTrace], DebugStub_Const_StepTrigger_Out
        ; EAX = .CallerEBP
        Mov EAX, DWORD [DebugStub_CallerEBP]
        ; .BreakEBP = EAX
        Mov DWORD [DebugStub_BreakEBP], EAX
	    ; goto Done
	; }

    ; goto WaitCmd

; Done:
DebugStub_Done:
    ; AckCommand()
    Call DebugStub_AckCommand
    ; .DebugStatus = #Status_Run
    Mov [DebugStub_DebugStatus], DebugStub_Const_Status_Run
; }

