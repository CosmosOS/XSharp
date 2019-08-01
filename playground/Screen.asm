; namespace DebugStub

; var DebugWaitMsg = 'Waiting for debugger connection...'

; //! %ifndef Exclude_Memory_Based_Console
%ifndef Exclude_Memory_Based_Console

; const VidBase = $B8000
DebugStub_Const_VidBase equ 753664

; function Cls {
DebugStub_Cls:
    ; ESI = #VidBase
    Mov ESI, DebugStub_Const_VidBase

	; while ESI < $B8FA0 {
		; [ESI] = $00
		Mov DWORD [ESI], 0x0
		; ESI++
		Inc ESI

		; [ESI] = $0A
		Mov DWORD [ESI], 0xA
		; ESI++
		Inc ESI
	; }
; }

; function DisplayWaitMsg {
DebugStub_DisplayWaitMsg:
	; ESI = @.DebugWaitMsg
	Mov ESI, DebugStub_DebugWaitMsg

    ; EDI = #VidBase
    Mov EDI, DebugStub_Const_VidBase
    ; EDI += 1640
    Add EDI, 0x668

    ; while byte [ESI] != 0 {
		; AL = [ESI]
		Mov AL, BYTE [ESI]
		; [EDI] = AL
		Mov BYTE [EDI], AL
		; ESI++
		Inc ESI
		; EDI += 2
		Add EDI, 0x2
	; }
; }

; //! %endif
%endif
