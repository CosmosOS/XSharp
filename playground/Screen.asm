; namespace DebugStub

; var DebugWaitMsg = 'Waiting for debugger connection...'
DebugStub_DebugWaitMsg:
		db 87, 97, 105, 116, 105, 110, 103, 32, 102, 111, 114, 32, 100, 101, 98, 117, 103, 103, 101, 114, 32, 99, 111, 110, 110, 101, 99, 116, 105, 111, 110, 46, 46, 46

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
	DebugStub_Cls_Block1_End:
; }
DebugStub_Cls_Exit:
Mov DWORD [INTS_LastKnownAddress], DebugStub_Cls_Exit
Ret 

; function DisplayWaitMsg {
DebugStub_DisplayWaitMsg:
	; ESI = @.DebugWaitMsg
	Mov ESI, DebugStub_DebugWaitMsg

    ; EDI = #VidBase
    Mov EDI, DebugStub_Const_VidBase
    ; 10 lines down, 20 cols in (10 * 80 + 20) * 2)
    ; EDI += 1640
    Add EDI, 0x668

    ; Read and copy string till 0 terminator
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
	DebugStub_DisplayWaitMsg_Block1_End:
; }
DebugStub_DisplayWaitMsg_Exit:
Mov DWORD [INTS_LastKnownAddress], DebugStub_DisplayWaitMsg_Exit
Ret 

; //! %endif
%endif
