; namespace DebugStub

; var DebugWaitMsg = 'Waiting for debugger connection...'

; //! %ifndef Exclude_Memory_Based_Console
%ifndef Exclude_Memory_Based_Console

; const VidBase = $B8000

; function Cls {
    ; ESI = #VidBase

	; End of Video Area
	; VidBase + 25 * 80 * 2 = B8FA0
	; while ESI < $B8FA0 {
		; Text
		; [ESI] = $00
		; ESI++

		; Colour
		; [ESI] = $0A
		; ESI++
	; }
; }

; function DisplayWaitMsg {
	; ESI = @.DebugWaitMsg

    ; EDI = #VidBase
    ; 10 lines down, 20 cols in (10 * 80 + 20) * 2)
    ; EDI += 1640

    ; Read and copy string till 0 terminator
    ; while byte [ESI] != 0 {
		; AL = [ESI]
		; [EDI] = AL
		; ESI++
		; EDI += 2
	; }
; }

; //! %endif
%endif
