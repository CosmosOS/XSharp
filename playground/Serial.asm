; namespace DebugStub


; //! %ifndef Exclude_IOPort_Based_SerialInit
%ifndef Exclude_IOPort_Based_SerialInit

; function InitSerial {
DebugStub_InitSerial:
  ; DX = 1
  Mov DX, 0x1
	; AL = 0
	Mov AL, 0x0
  ; WriteRegister()
  Call DebugStub_WriteRegister

	; DX = 3
	Mov DX, 0x3
	; AL = $80
	Mov AL, 0x80
	; WriteRegister()
	Call DebugStub_WriteRegister

	; DX = 0
	Mov DX, 0x0
	; AL = $01
	Mov AL, 0x1
	; WriteRegister()
	Call DebugStub_WriteRegister

	; DX = 1
	Mov DX, 0x1
	; AL = $00
	Mov AL, 0x0
	; WriteRegister()
	Call DebugStub_WriteRegister

	; DX = 3
	Mov DX, 0x3
	; AL = $03
	Mov AL, 0x3
	; WriteRegister()
	Call DebugStub_WriteRegister

  ; DX = 2
  Mov DX, 0x2
	; AL = $C7
	Mov AL, 0xC7
	; WriteRegister()
	Call DebugStub_WriteRegister

	; DX = 4
	Mov DX, 0x4
	; AL = $03
	Mov AL, 0x3
	; WriteRegister()
	Call DebugStub_WriteRegister
; }

; function ComReadAL {
DebugStub_ComReadAL:
	; DX = 5
	Mov DX, 0x5
; Wait:
DebugStub_Wait:
    ; ReadRegister()
    Call DebugStub_ReadRegister
    ; AL test $01
    Test AL, 0x1
    ; if 0 goto Wait

	; DX = 0
	Mov DX, 0x0
  ; ReadRegister()
  Call DebugStub_ReadRegister
; }

; function ComWrite8 {
DebugStub_ComWrite8:

	; DX = 5
	Mov DX, 0x5

; Wait:
DebugStub_Wait:
    ; ReadRegister()
    Call DebugStub_ReadRegister
	  ; AL test $20
	  Test AL, 0x20
	  ; if 0 goto Wait

	; DX = 0
	Mov DX, 0x0
  ; AL = [ESI]
  Mov AL, BYTE [ESI]
	; WriteRegister()
	Call DebugStub_WriteRegister

	; ESI++
	Inc ESI
; }

; //! %endif
%endif
