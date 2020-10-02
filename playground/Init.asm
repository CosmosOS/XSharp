; namespace DebugStub

; var DebugBPs dword[256]
DebugStub_DebugBPs dd 0
; var MaxBPId
DebugStub_MaxBPId dd 0

; Called before Kernel runs. Inits debug stub, etc
; function Init {
DebugStub_Init:
    ; Cls()
    Call DebugStub_Cls
	; Display message before even trying to init serial
    ; DisplayWaitMsg()
    Call DebugStub_DisplayWaitMsg
    ; InitSerial()
    Call DebugStub_InitSerial
    ; WaitForDbgHandshake()
    Call DebugStub_WaitForDbgHandshake
    ; Cls()
    Call DebugStub_Cls
; }
DebugStub_Init_Exit:
Mov DWORD [INTS_LastKnownAddress], DebugStub_Init_Exit
Ret 

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
	DebugStub_WaitForSignature_Block1_End:
; }
DebugStub_WaitForSignature_Exit:
Mov DWORD [INTS_LastKnownAddress], DebugStub_WaitForSignature_Exit
Ret 

; QEMU (and possibly others) send some garbage across the serial line first.
; Actually they send the garbage inbound, but garbage could be inbound as well so we
; keep this.
; To work around this we send a signature. DC then discards everything before the signature.
; QEMU has other serial issues too, and we dont support it anymore, but this signature is a good
; feature so we kept it.
; function WaitForDbgHandshake {
DebugStub_WaitForDbgHandshake:
    ; "Clear" the UART out
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

    ; Cosmos.Debug.Consts.Consts.SerialSignature
	; +#Signature
	Push DebugStub_Const_Signature
    ; ESI = ESP
    Mov ESI, ESP

    ; ComWrite32()
    Call DebugStub_ComWrite32

    ; Restore ESP, we actually dont care about EAX or the value on the stack anymore.
    ; -EAX
    Pop EAX

    ; We could use the signature as the start signal, but I prefer
    ; to keep the logic separate, especially in DC.
	; Send the actual started signal
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
DebugStub_WaitForDbgHandshake_Exit:
Mov DWORD [INTS_LastKnownAddress], DebugStub_WaitForDbgHandshake_Exit
Ret 

; //! %ifndef Exclude_Dummy_Hooks
%ifndef Exclude_Dummy_Hooks
; function Hook_OnHandshakeCompleted {
DebugStub_Hook_OnHandshakeCompleted:
; }
DebugStub_Hook_OnHandshakeCompleted_Exit:
Mov DWORD [INTS_LastKnownAddress], DebugStub_Hook_OnHandshakeCompleted_Exit
Ret 
; //! %endif
%endif
