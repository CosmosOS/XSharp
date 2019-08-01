; namespace DebugStub

; var DebugBPs dword[256]
; var MaxBPId
DebugStub_MaxBPId dd 0

; function Init {
DebugStub_Init:
    ; Cls()
    Call DebugStub_Cls
    ; DisplayWaitMsg()
    Call DebugStub_DisplayWaitMsg
    ; InitSerial()
    Call DebugStub_InitSerial
    ; WaitForDbgHandshake()
    Call DebugStub_WaitForDbgHandshake
    ; Cls()
    Call DebugStub_Cls
; }

; function WaitForSignature {
DebugStub_WaitForSignature:
    ; EBX = 0
    Mov EBX, 0x0
	; while EBX != #Signature {
		; ComReadAL()
		Call DebugStub_ComReadAL
		; BL = AL
		Mov BL, AL
		; EBX ~> 8
		Ror EBX, 0x8
	; }
; }

; function WaitForDbgHandshake {
DebugStub_WaitForDbgHandshake:
    ; AL = 0
    Mov AL, 0x0
    ; ComWriteAL()
    Call DebugStub_ComWriteAL
	; AL = 0
	Mov AL, 0x0
    ; ComWriteAL()
    Call DebugStub_ComWriteAL
	; AL = 0
	Mov AL, 0x0
    ; ComWriteAL()
    Call DebugStub_ComWriteAL

	; +#Signature
	Push DebugStub_Const_Signature
    ; ESI = ESP
    Mov ESI, ESP

    ; ComWrite32()
    Call DebugStub_ComWrite32

    ; -EAX
    Pop EAX

    ; AL = #Ds2Vs_Started
    Mov AL, DebugStub_Const_Ds2Vs_Started
    ; ComWriteAL()
    Call DebugStub_ComWriteAL

    ; WaitForSignature()
    Call DebugStub_WaitForSignature
    ; ProcessCommandBatch()
    Call DebugStub_ProcessCommandBatch
	; Hook_OnHandshakeCompleted()
	Call DebugStub_Hook_OnHandshakeCompleted
; }

; //! %ifndef Exclude_Dummy_Hooks
%ifndef Exclude_Dummy_Hooks
; function Hook_OnHandshakeCompleted {
DebugStub_Hook_OnHandshakeCompleted:
; }
; //! %endif
%endif
