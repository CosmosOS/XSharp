

; namespace DebugStub

; Interrupt TracerEntry {

; //! cli
cli


	; +All
	PushAD 
; .PushAllPtr = ESP
Mov DWORD [DebugStub_PushAllPtr], ESP
; .CallerEBP = EBP
Mov DWORD [DebugStub_CallerEBP], EBP

; EBP = ESP
Mov EBP, ESP
; EBP += 32
Add EBP, 0x20
; EAX = [EBP]
Mov EAX, DWORD [EBP]

; EBP += 12
Add EBP, 0xC
; .CallerESP = EBP
Mov DWORD [DebugStub_CallerESP], EBP


; EBX = EAX
Mov EBX, EAX
; //! MOV EAX, DR6
MOV EAX, DR6
; EAX & $4000
; if EAX != $4000 {
	; EBX--
	Dec EBX
; }
; EAX = EBX
Mov EAX, EBX

; .CallerEIP = EAX
Mov DWORD [DebugStub_CallerEIP], EAX

	; Executing()
	Call DebugStub_Executing

; -All
PopAD 

; //! sti
sti

; }
