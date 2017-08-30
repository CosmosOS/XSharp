	; EAX = ESI[4]
    ; EAX = [ESI + 4]
    ; EAX = ESI
	; EAX = [ESI]
; Test Comment
    ; AX = 0

    ; +EAX
    ; //! nop
    nop
	; EAX++

    ; AH = 0
    ; AH = $FF
    ; AX = 0
    ; AX = $FFFF
	; EAX = 0
	; EAX = $FFFF
	; EAX = $FFFFFFFF
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
; Above here is temp Test Area
