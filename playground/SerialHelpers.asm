; namespace DebugStub

; Helper functions which make it easier to use serial stuff

; function ComReadEAX {
DebugStub_ComReadEAX:
	; repeat 4 times {
		; ComReadAL()
		Call DebugStub_ComReadAL
		; EAX ~> 8
	; }
; }

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
; function ComRead16 {
DebugStub_ComRead16:
	; repeat 2 times {
		; ComRead8()
		Call DebugStub_ComRead8
	; }
; }
; function ComRead32 {
DebugStub_ComRead32:
	; repeat 4 times {
		; ComRead8()
		Call DebugStub_ComRead8
	; }
; }

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

; function ComWrite16 {
DebugStub_ComWrite16:
	; ComWrite8()
	Call DebugStub_ComWrite8
	; ComWrite8()
	Call DebugStub_ComWrite8
; }
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
; function ComWriteX {
DebugStub_ComWriteX:
; More:
DebugStub_More:
	; ComWrite8()
	Call DebugStub_ComWrite8
	; ECX--
	Dec ECX
	; if !0 goto More
; }
