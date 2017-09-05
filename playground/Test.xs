namespace DebugStub

AX = [EBX]
ESI = @.CallerESP
EAX = @.CallerESP
AX = @.CallerESP
ESI = .CallerESP
[ESI] = $00


//END
; Generated at 9/4/2017 5:39:11 PM


mov AX, word [EBX]
mov ESI, CallerESP
mov EAX, CallerESP
mov AX, CallerESP
mov ESI, dword [CallerESP]
mov dword [ESI], 0x0
