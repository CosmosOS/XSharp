; namespace DebugStub

; Temp Test Area
    ; //! nop
    nop
    ; AH = 0
    Mov AH, 0x0
    ; AX = 0
    Mov AX, 0x0
	; EAX = 0
	Mov EAX, 0x0
    ; NOP
    NOP 
    ; return
    RET 
	; EAX = $FFFF
	Mov EAX, 0xFFFF
	; EAX = $FFFFFFFF
	Mov EAX, 0xFFFFFFFF
	; +All
	Pushad 
	; -All
	Popad 

; Modifies: AL, DX (ComReadAL)
; Returns: AL
