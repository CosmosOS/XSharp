; namespace DebugStub

; Helper functions which make it easier to use serial stuff

; function ComReadEAX {
DebugStub_ComReadEAX:
	; repeat 4 times {
		; ComReadAL()
		Call DebugStub_ComReadAL
		; EAX ~> 8
		Ror EAX, 0x8
	; }
	DebugStub_ComReadEAX_Block1_End:
; }
DebugStub_ComReadEAX_Exit:
Mov DWORD [INTS_LastKnownAddress], DebugStub_ComReadEAX_Exit
Ret 

; Input: EDI
; Output: [EDI]
; Modified: AL, DX, EDI (+1)
; Reads a byte into [EDI] and does EDI + 1
; function ComRead8  {
DebugStub_ComRead8:
    ; ComReadAL()
    Call DebugStub_ComReadAL
    ; [EDI] = AL
    Mov BYTE [EDI], AL
    ; EDI += 1
    Add EDI, 0x1
; }
DebugStub_ComRead8_Exit:
Mov DWORD [INTS_LastKnownAddress], DebugStub_ComRead8_Exit
Ret 
; function ComRead16 {
DebugStub_ComRead16:
	; repeat 2 times {
		; ComRead8()
		Call DebugStub_ComRead8
	; }
	DebugStub_ComRead16_Block1_End:
; }
DebugStub_ComRead16_Exit:
Mov DWORD [INTS_LastKnownAddress], DebugStub_ComRead16_Exit
Ret 
; function ComRead32 {
DebugStub_ComRead32:
	; repeat 4 times {
		; ComRead8()
		Call DebugStub_ComRead8
	; }
	DebugStub_ComRead32_Block1_End:
; }
DebugStub_ComRead32_Exit:
Mov DWORD [INTS_LastKnownAddress], DebugStub_ComRead32_Exit
Ret 

; Input: AL
; Output: None
; Modifies: EDX
; function ComWriteAL {
DebugStub_ComWriteAL:
	; +ESI
	Push ESI
    ; +EAX
    Push EAX
	; ESI = ESP
	Mov ESI, ESP
    ; ComWrite8()
    Call DebugStub_ComWrite8
    ; Is a local var, cant use Return(4). X// issues the return.
    ; This also allows the function to preserve EAX.
    ; -EAX
    Pop EAX
	; -ESI
	Pop ESI
; }
DebugStub_ComWriteAL_Exit:
Mov DWORD [INTS_LastKnownAddress], DebugStub_ComWriteAL_Exit
Ret 
; function ComWriteAX {
DebugStub_ComWriteAX:
    ; Input: AX
    ; Output: None
    ; Modifies: EDX, ESI
    ; +EAX
    Push EAX
    ; ESI = ESP
    Mov ESI, ESP
    ; ComWrite16()
    Call DebugStub_ComWrite16
    ; Is a local var, cant use Return(4). X// issues the return.
    ; This also allow the function to preserve EAX.
    ; -EAX
    Pop EAX
; }
DebugStub_ComWriteAX_Exit:
Mov DWORD [INTS_LastKnownAddress], DebugStub_ComWriteAX_Exit
Ret 
; function ComWriteEAX {
DebugStub_ComWriteEAX:
    ; Input: EAX
    ; Output: None
    ; Modifies: EDX, ESI
    ; +EAX
    Push EAX
    ; ESI = ESP
    Mov ESI, ESP
    ; ComWrite32()
    Call DebugStub_ComWrite32
    ; Is a local var, cant use Return(4). X// issues the return.
    ; This also allow the function to preserve EAX.
    ; -EAX
    Pop EAX
; }
DebugStub_ComWriteEAX_Exit:
Mov DWORD [INTS_LastKnownAddress], DebugStub_ComWriteEAX_Exit
Ret 

; function ComWrite16 {
DebugStub_ComWrite16:
	; ComWrite8()
	Call DebugStub_ComWrite8
	; ComWrite8()
	Call DebugStub_ComWrite8
; }
DebugStub_ComWrite16_Exit:
Mov DWORD [INTS_LastKnownAddress], DebugStub_ComWrite16_Exit
Ret 
; function ComWrite32 {
DebugStub_ComWrite32:
	; ComWrite8()
	Call DebugStub_ComWrite8
	; ComWrite8()
	Call DebugStub_ComWrite8
	; ComWrite8()
	Call DebugStub_ComWrite8
	; ComWrite8()
	Call DebugStub_ComWrite8
; }
DebugStub_ComWrite32_Exit:
Mov DWORD [INTS_LastKnownAddress], DebugStub_ComWrite32_Exit
Ret 
; function ComWriteX {
DebugStub_ComWriteX:
; More:
DebugStub_ComWriteX_More:
	; ComWrite8()
	Call DebugStub_ComWrite8
	; ECX--
	Dec ECX
	; if !0 goto More
; }
DebugStub_ComWriteX_Exit:
Mov DWORD [INTS_LastKnownAddress], DebugStub_ComWriteX_Exit
Ret 
