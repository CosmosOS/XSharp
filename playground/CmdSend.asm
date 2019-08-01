; namespace DebugStub

; function SendRegisters {
DebugStub_SendRegisters:
    ; AL = #Ds2Vs_Registers
    Mov AL, DebugStub_Const_Ds2Vs_Registers
    ; ComWriteAL()
    Call DebugStub_ComWriteAL

    ; ESI = .PushAllPtr
    Mov ESI, DWORD [DebugStub_PushAllPtr]
    ; ECX = 32
    Mov ECX, 0x20
    ; ComWriteX()
    Call DebugStub_ComWriteX

    ; ESI = @.CallerESP
    Mov ESI, DebugStub_CallerESP
    ; ComWrite32()
    Call DebugStub_ComWrite32

    ; ESI = @.CallerEIP
    Mov ESI, DebugStub_CallerEIP
    ; ComWrite32()
    Call DebugStub_ComWrite32
; }

; function SendFrame {
DebugStub_SendFrame:
    ; AL = #Ds2Vs_Frame
    Mov AL, DebugStub_Const_Ds2Vs_Frame
    ; ComWriteAL()
    Call DebugStub_ComWriteAL

    ; EAX = 32
    Mov EAX, 0x20
    ; ComWriteAX()
    Call DebugStub_ComWriteAX

    ; ESI = .CallerEBP
    Mov ESI, DWORD [DebugStub_CallerEBP]
    ; ESI += 8
    Add ESI, 0x8
    ; ECX = 32
    Mov ECX, 0x20
    ; ComWriteX()
    Call DebugStub_ComWriteX
; }

; function SendCommandOnChannel{
DebugStub_SendCommandOnChannel:
  ; +All
  PushAD 
    ; ComWriteAL()
    Call DebugStub_ComWriteAL
  ; -All
  PopAD 

  ; AL = BL
  Mov AL, BL

  ; +All
  PushAD 
    ; ComWriteAL()
    Call DebugStub_ComWriteAL
  ; -All
  PopAD 

  ; +All
  PushAD 
    ; EAX = ECX
    Mov EAX, ECX
    ; ComWriteEAX()
    Call DebugStub_ComWriteEAX
  ; -All
  PopAD 

    ; while ECX != 0 {
        ; ComWrite8()
        Call DebugStub_ComWrite8
        ; ECX--
        Dec ECX
    ; }
; }

; function SendStack {
DebugStub_SendStack:
    ; AL = #Ds2Vs_Stack
    Mov AL, DebugStub_Const_Ds2Vs_Stack
    ; ComWriteAL()
    Call DebugStub_ComWriteAL

    ; ESI = .CallerESP
    Mov ESI, DWORD [DebugStub_CallerESP]
    ; EAX = .CallerEBP
    Mov EAX, DWORD [DebugStub_CallerEBP]
    ; EAX -= ESI
    Sub EAX, ESI
    ; ComWriteAX()
    Call DebugStub_ComWriteAX

    ; ESI = .CallerESP
    Mov ESI, DWORD [DebugStub_CallerESP]
    ; while ESI != .CallerEBP {
        ; ComWrite8()
        Call DebugStub_ComWrite8
    ; }
; }

; function SendMethodContext {
DebugStub_SendMethodContext:
    ; +All
    PushAD 

    ; AL = #Ds2Vs_MethodContext
    Mov AL, DebugStub_Const_Ds2Vs_MethodContext
    ; ComWriteAL()
    Call DebugStub_ComWriteAL

    ; ESI = .CallerEBP
    Mov ESI, DWORD [DebugStub_CallerEBP]

    ; ComReadEAX()
    Call DebugStub_ComReadEAX
    ; ESI += EAX
    Add ESI, EAX
    ; ComReadEAX()
    Call DebugStub_ComReadEAX
    ; ECX = EAX
    Mov ECX, EAX


    ; while ECX != 0 {
        ; ComWrite8()
        Call DebugStub_ComWrite8
        ; ECX--
        Dec ECX
    ; }

; Exit:
DebugStub_Exit:
    ; -All
    PopAD 
; }

; function SendMemory {
DebugStub_SendMemory:
    ; +All
    PushAD 

    ; AL = #Ds2Vs_MemoryData
    Mov AL, DebugStub_Const_Ds2Vs_MemoryData
    ; ComWriteAL()
    Call DebugStub_ComWriteAL

    ; ComReadEAX()
    Call DebugStub_ComReadEAX
    ; ESI = EAX
    Mov ESI, EAX
    ; ComReadEAX()
    Call DebugStub_ComReadEAX
    ; ECX = EAX
    Mov ECX, EAX

    ; while ECX != 0 {
        ; ComWrite8()
        Call DebugStub_ComWrite8
        ; ECX--
        Dec ECX
    ; }

; Exit:
DebugStub_Exit:
    ; -All
    PopAD 
; }

; function SendTrace {
DebugStub_SendTrace:
    ; AL = #Ds2Vs_BreakPoint
    Mov AL, DebugStub_Const_Ds2Vs_BreakPoint
    ; if dword .DebugStatus = #Status_Run {
        ; AL = #Ds2Vs_TracePoint
        Mov AL, DebugStub_Const_Ds2Vs_TracePoint
    ; }
    ; ComWriteAL()
    Call DebugStub_ComWriteAL

    ; ESI = @.CallerEIP
    Mov ESI, DebugStub_CallerEIP
    ; ComWrite32()
    Call DebugStub_ComWrite32
; }

; function SendText {
DebugStub_SendText:
; +EBP
Push EBP
; EBP = ESP
Mov EBP, ESP
    ; +All
    PushAD 
    ; AL = #Ds2Vs_Message
    Mov AL, DebugStub_Const_Ds2Vs_Message
    ; ComWriteAL()
    Call DebugStub_ComWriteAL

    ; ESI = EBP
    Mov ESI, EBP
    ; ESI += 12
    Add ESI, 0xC
    ; ECX = [ESI]
    Mov ECX, DWORD [ESI]
    ; ComWrite16()
    Call DebugStub_ComWrite16

    ; ESI = [EBP + 8]
    Mov ESI, DWORD [EBP + 8]
; WriteChar:
DebugStub_WriteChar:
    ; if ECX = 0 goto Finalize
    ; ComWrite8()
    Call DebugStub_ComWrite8
    ; ECX--
    Dec ECX
    ; ESI++
    Inc ESI
    ; goto WriteChar

; Finalize:
DebugStub_Finalize:
    ; -All
    PopAD 
  ; -EBP
  Pop EBP
; }

; function SendSimpleNumber {
DebugStub_SendSimpleNumber:
; +EBP
Push EBP
; EBP = ESP
Mov EBP, ESP
    ; +All
    PushAD 
    ; AL = #Ds2Vs_SimpleNumber
    Mov AL, DebugStub_Const_Ds2Vs_SimpleNumber
    ; ComWriteAL()
    Call DebugStub_ComWriteAL

    ; EAX = [EBP + 8]
    Mov EAX, DWORD [EBP + 8]
    ; ComWriteEAX()
    Call DebugStub_ComWriteEAX

    ; -All
    PopAD 
  ; -EBP
  Pop EBP
; }

; function SendKernelPanic {
DebugStub_SendKernelPanic:
; +EBP
Push EBP
; EBP = ESP
Mov EBP, ESP
    ; +All
    PushAD 
    ; AL = #Ds2Vs_KernelPanic
    Mov AL, DebugStub_Const_Ds2Vs_KernelPanic
    ; ComWriteAL()
    Call DebugStub_ComWriteAL

    ; EAX = [EBP + 8]
    Mov EAX, DWORD [EBP + 8]
    ; ComWriteEAX()
    Call DebugStub_ComWriteEAX

	; SendCoreDump()
	Call DebugStub_SendCoreDump
    ; -All
    PopAD 
  ; -EBP
  Pop EBP
; }

; function SendSimpleLongNumber {
DebugStub_SendSimpleLongNumber:
  ; +EBP
  Push EBP
  ; EBP = ESP
  Mov EBP, ESP
  ; +All
  PushAD 

  ; AL = #Ds2Vs_SimpleLongNumber
  Mov AL, DebugStub_Const_Ds2Vs_SimpleLongNumber
  ; ComWriteAL()
  Call DebugStub_ComWriteAL

  ; EAX = [EBP + 8]
  Mov EAX, DWORD [EBP + 8]
  ; ComWriteEAX()
  Call DebugStub_ComWriteEAX
  ; EAX = [EBP + 12]
  Mov EAX, DWORD [EBP + 12]
  ; ComWriteEAX()
  Call DebugStub_ComWriteEAX

  ; -All
  PopAD 
  ; -EBP
  Pop EBP
; }

; function SendComplexNumber {
DebugStub_SendComplexNumber:
  ; +EBP
  Push EBP
  ; EBP = ESP
  Mov EBP, ESP
  ; +All
  PushAD 

  ; AL = #Ds2Vs_ComplexNumber
  Mov AL, DebugStub_Const_Ds2Vs_ComplexNumber
  ; ComWriteAL()
  Call DebugStub_ComWriteAL

  ; EAX = [EBP+8]
  Mov EAX, DWORD [EBP + 8]
  ; ComWriteEAX()
  Call DebugStub_ComWriteEAX

  ; -All
  PopAD 
  ; -EBP
  Pop EBP
; }

; function SendComplexLongNumber {
DebugStub_SendComplexLongNumber:
  ; +EBP
  Push EBP
  ; EBP = ESP
  Mov EBP, ESP
  ; +All
  PushAD 

  ; AL = #Ds2Vs_ComplexLongNumber
  Mov AL, DebugStub_Const_Ds2Vs_ComplexLongNumber
  ; ComWriteAL()
  Call DebugStub_ComWriteAL

  ; EAX = [EBP+8]
  Mov EAX, DWORD [EBP + 8]
  ; ComWriteEAX()
  Call DebugStub_ComWriteEAX
  ; EAX = [EBP+12]
  Mov EAX, DWORD [EBP + 12]
  ; ComWriteEAX()
  Call DebugStub_ComWriteEAX

  ; -All
  PopAD 
  ; -EBP
  Pop EBP
; }

; function SendPtr {
DebugStub_SendPtr:
    ; AL = #Ds2Vs_Pointer
    Mov AL, DebugStub_Const_Ds2Vs_Pointer
    ; ComWriteAL()
    Call DebugStub_ComWriteAL

    ; ESI = [EBP+8]
    Mov ESI, DWORD [EBP + 8]
    ; ComWrite32()
    Call DebugStub_ComWrite32
; }

; function SendStackCorruptionOccurred {
DebugStub_SendStackCorruptionOccurred:
    ; AL = #Ds2Vs_StackCorruptionOccurred
    Mov AL, DebugStub_Const_Ds2Vs_StackCorruptionOccurred
    ; ComWriteAL()
    Call DebugStub_ComWriteAL

    ; ESI = @.CallerEIP
    Mov ESI, DebugStub_CallerEIP
    ; ComWrite32()
    Call DebugStub_ComWrite32
; }

; function SendStackOverflowOccurred {
DebugStub_SendStackOverflowOccurred:
    ; AL = #Ds2Vs_StackOverflowOccurred
    Mov AL, DebugStub_Const_Ds2Vs_StackOverflowOccurred
    ; ComWriteAL()
    Call DebugStub_ComWriteAL

    ; ESI = @.CallerEIP
    Mov ESI, DebugStub_CallerEIP
    ; ComWrite32()
    Call DebugStub_ComWrite32
; }

; function SendInterruptOccurred {
DebugStub_SendInterruptOccurred:
	; +EAX
	Push EAX

		; AL = #Ds2Vs_InterruptOccurred
		Mov AL, DebugStub_Const_Ds2Vs_InterruptOccurred
		; ComWriteAL()
		Call DebugStub_ComWriteAL

    ; -EAX
    Pop EAX
	; ComWriteEAX()
	Call DebugStub_ComWriteEAX
; }

; function SendNullReferenceOccurred {
DebugStub_SendNullReferenceOccurred:
    ; AL = #Ds2Vs_NullReferenceOccurred
    Mov AL, DebugStub_Const_Ds2Vs_NullReferenceOccurred
    ; ComWriteAL()
    Call DebugStub_ComWriteAL

    ; ESI = @.CallerEIP
    Mov ESI, DebugStub_CallerEIP
    ; ComWrite32()
    Call DebugStub_ComWrite32
; }

; function SendMessageBox {
DebugStub_SendMessageBox:
    ; AL = #Ds2Vs_MessageBox
    Mov AL, DebugStub_Const_Ds2Vs_MessageBox
    ; ComWriteAL()
    Call DebugStub_ComWriteAL

    ; ESI = EBP
    Mov ESI, EBP
    ; ESI += 12
    Add ESI, 0xC
    ; ECX = [ESI]
    Mov ECX, DWORD [ESI]
    ; ComWrite16()
    Call DebugStub_ComWrite16

    ; ESI = [EBP+8]
    Mov ESI, DWORD [EBP + 8]
; WriteChar:
DebugStub_WriteChar:
    ; if ECX = 0 return
    ; ComWrite8()
    Call DebugStub_ComWrite8
    ; ECX--
    Dec ECX
    ; ESI++
    Inc ESI
    ; goto WriteChar
; }

; function SendCoreDump {
DebugStub_SendCoreDump:
    ; +EAX
    Push EAX
    ; +EBX
    Push EBX
    ; +ECX
    Push ECX
    ; +EDX
    Push EDX
    ; +EDI
    Push EDI
    ; +ESI
    Push ESI
    ; EAX = @.CallerEBP
    Mov EAX, DebugStub_CallerEBP
    ; +EAX
    Push EAX
    ; EAX = @.CallerEIP
    Mov EAX, DebugStub_CallerEIP
    ; +EAX
    Push EAX
    ; EAX = @.CallerESP
    Mov EAX, DebugStub_CallerESP
    ; +EAX
    Push EAX
    ; ECX = 36
    Mov ECX, 0x24
    ; EAX = EBP
    Mov EAX, EBP
    ; while EAX != 0 {
        ; EAX -= 4
        Sub EAX, 0x4
        ; +EAX
        Push EAX
        ; ECX += 4
        Add ECX, 0x4
        ; EAX = [EAX]
        Mov EAX, DWORD [EAX]
    ; }

	; AL = #Ds2Vs_CoreDump
	Mov AL, DebugStub_Const_Ds2Vs_CoreDump
	; ComWriteAL()
	Call DebugStub_ComWriteAL
    ; EAX = ECX
    Mov EAX, ECX
    ; ComWriteAX()
    Call DebugStub_ComWriteAX
    ; while ECX != 0 {
        ; -EAX
        Pop EAX
        ; ComWriteEAX()
        Call DebugStub_ComWriteEAX
        ; ECX--
        Dec ECX
    ; }
; }
