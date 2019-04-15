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

	; End of Video Area
	; VidBase + 25 * 80 * 2 = B8FA0
	; while ESI < $B8FA0 {
		; Text
		; [ESI] = $00
		Mov DWORD [ESI], 0x0
		; ESI++
		Inc ESI

		; Colour
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
    ; 10 lines down, 20 cols in (10 * 80 + 20) * 2)
    ; EDI += 1640

    ; Read and copy string till 0 terminator
    ; while byte [ESI] != 0 {
		; AL = [ESI]
		Mov AL, BYTE [ESI]
		; [EDI] = AL
		Mov BYTE [EDI], AL
		; ESI++
		Inc ESI
		; EDI += 2
	; }
; }

; //! %endif
%endif
