; Temp Test Area
    ; +EAX
    ; //! nop
    nop
	; EAX++

    ; AH = 0
    Mov AH, 0x0
    ; AH = $FF
    Mov AH, 0xFF
    ; AX = 0
    Mov AX, 0x0
    ; AX = $FFFF
    Mov AX, 0xFFFF
	; EAX = 0
	Mov EAX, 0x0
	; EAX = $FFFF
	Mov EAX, 0xFFFF
	; EAX = $FFFFFFFF
	Mov EAX, 0xFFFFFFFF
    ; NOP
    NOP 
    ; return
    Ret 
	; +All
	PushAD 
	; -All
	PopAD 

	; const i = 'Test \'string\''
	; EAX = ~ECX

; namespace DebugStub

; Modifies: AL, DX (ComReadAL)
; Returns: AL
; function ProcessCommand {
    ; ComReadAL()
    ; Some callers expect AL to be returned, so we preserve it
    ; in case any commands modify AL.
    ; We push EAX to keep stack aligned.
    ; +EAX
