    ; AX = 0
    Mov AX, 0x0

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
