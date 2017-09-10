; namespace DebugStub

; function SendRegisters {
    ; Send the actual started signal
    ; AL = #Ds2Vs_Registers
    Mov AL, DebugStub_Const_Ds2Vs_Registers
    ; ComWriteAL()

    ; ESI = .PushAllPtr
    Mov ESI, DWORD [DebugStub_Var_PushAllPtr]
    ; ECX = 32
    Mov ECX, 0x20
    ; ComWriteX()

    ; ESI = @.CallerESP
    Mov ESI, DebugStub_Var_CallerESP
    ; ComWrite32()

    ; ESI = @.CallerEIP
    Mov ESI, DebugStub_Var_CallerEIP
    ; ComWrite32()
; }

; function SendFrame {
    ; AL = #Ds2Vs_Frame
    Mov AL, DebugStub_Const_Ds2Vs_Frame
    ; ComWriteAL()

    ; EAX = 32
    Mov EAX, 0x20
    ; ComWriteAX()

    ; ESI = .CallerEBP
    Mov ESI, DWORD [DebugStub_Var_CallerEBP]
    ; Dont transmit EIP or old EBP
    ; ESI += 8
    ; ECX = 32
    Mov ECX, 0x20
    ; ComWriteX()
; }

; AL contains channel
; BL contains command
; ESI contains data start pointer
; ECX contains number of bytes to send as command data
; function SendCommandOnChannel{
  ; +All
  PushAD 
    ; ComWriteAL()
  ; -All
  PopAD 

  ; AL = BL
  Mov AL, BL

  ; +All
  PushAD 
    ; ComWriteAL()
  ; -All
  PopAD 

  ; +All
  PushAD 
    ; EAX = ECX
    Mov EAX, ECX
    ; ComWriteEAX()
  ; -All
  PopAD 

  ; now ECX contains size of data (count)
    ; ESI contains address
    ; while ECX != 0 {
        ; ComWrite8()
        ; ECX--
        Dec ECX
    ; }
; }

; function SendStack {
    ; AL = #Ds2Vs_Stack
    Mov AL, DebugStub_Const_Ds2Vs_Stack
    ; ComWriteAL()

    ; Send size of bytes
    ; ESI = .CallerESP
    Mov ESI, DWORD [DebugStub_Var_CallerESP]
    ; EAX = .CallerEBP
    Mov EAX, DWORD [DebugStub_Var_CallerEBP]
    ; EAX -= ESI
    ; ComWriteAX()

    ; Send actual bytes
    ; Need to reload ESI, WriteAXToCompPort modifies it
    ; ESI = .CallerESP
    Mov ESI, DWORD [DebugStub_Var_CallerESP]
    ; while ESI != .CallerEBP {
        ; ComWrite8()
    ; }
; }

; sends a stack value
; Serial Params:
; 1: x32 - offset relative to EBP
; 2: x32 - size of data to send
; function SendMethodContext {
    ; +All
    PushAD 

    ; AL = #Ds2Vs_MethodContext
    Mov AL, DebugStub_Const_Ds2Vs_MethodContext
    ; ComWriteAL()

    ; ESI = .CallerEBP
    Mov ESI, DWORD [DebugStub_Var_CallerEBP]

    ; offset relative to ebp
    ; size of data to send
    ; ComReadEAX()
    ; ESI += EAX
    ; ComReadEAX()
    ; ECX = EAX
    Mov ECX, EAX

    ; now ECX contains size of data (count)
    ; ESI contains relative to EBP

    ; while ECX != 0 {
        ; ComWrite8()
        ; ECX--
        Dec ECX
    ; }

; Exit:
    ; -All
    PopAD 
; }

; none
; saveregs
; frame
; sends a stack value
; Serial Params:
; 1: x32 - address
; 2: x32 - size of data to send
; function SendMemory {
    ; +All
    PushAD 

    ; AL = #Ds2Vs_MemoryData
    Mov AL, DebugStub_Const_Ds2Vs_MemoryData
    ; ComWriteAL()

    ; ComReadEAX()
    ; ESI = EAX
    Mov ESI, EAX
    ; ComReadEAX()
    ; ECX = EAX
    Mov ECX, EAX

    ; now ECX contains size of data (count)
    ; ESI contains address
    ; while ECX != 0 {
        ; ComWrite8()
        ; ECX--
        Dec ECX
    ; }

; Exit:
    ; -All
    PopAD 
; }

; Modifies: EAX, ESI
; function SendTrace {
    ; AL = #Ds2Vs_BreakPoint
    Mov AL, DebugStub_Const_Ds2Vs_BreakPoint
    ; If we are running, its a tracepoint, not a breakpoint.
    ; In future, maybe separate these into 2 methods
    ; if dword .DebugStatus = #Status_Run {
        ; AL = #Ds2Vs_TracePoint
        Mov AL, DebugStub_Const_Ds2Vs_TracePoint
    ; }
    ; ComWriteAL()

    ; Send Calling EIP.
    ; ESI = @.CallerEIP
    Mov ESI, DebugStub_Var_CallerEIP
    ; ComWrite32()
; }

; Input: Stack
; Output: None
; Modifies: EAX, ECX, EDX, ESI
; function SendText {
; +EBP
Push EBP
; EBP = ESP
Mov EBP, ESP
    ; +All
    PushAD 
    ; Write the type
    ; AL = #Ds2Vs_Message
    Mov AL, DebugStub_Const_Ds2Vs_Message
    ; ComWriteAL()

    ; Write Length
    ; ESI = EBP
    Mov ESI, EBP
    ; ESI += 12
    ; ECX = [ESI]
    Mov ECX, DWORD [ESI]
    ; ComWrite16()

    ; Address of string
    ; ESI = [EBP + 8]
    Mov ESI, DWORD [EBP + 8]
; WriteChar:
    ; if ECX = 0 goto Finalize
    ; ComWrite8()
    ; ECX--
    Dec ECX
    ; We are storing as 16 bits, but for now I will transmit 8 bits
    ; So we inc again to skip the 0
    ; ESI++
    Inc ESI
    ; goto WriteChar

    ; Write Length
    ; ESI = EBP
    ; ESI + 12
    ; ECX = [ESI]
    ; // Address of string
    ; ESI = [EBP + 8]
; Finalize:
    ; -All
    PopAD 
  ; -EBP
  Pop EBP
; }

; Input: Stack
; Output: None
; Modifies: EAX, ECX, EDX, ESI
; function SendSimpleNumber {
; +EBP
Push EBP
; EBP = ESP
Mov EBP, ESP
    ; +All
    PushAD 
    ; Write the type
    ; AL = #Ds2Vs_SimpleNumber
    Mov AL, DebugStub_Const_Ds2Vs_SimpleNumber
    ; ComWriteAL()

    ; Write value
    ; EAX = [EBP + 8]
    Mov EAX, DWORD [EBP + 8]
    ; ComWriteEAX()

    ; -All
    PopAD 
  ; -EBP
  Pop EBP
; }

; Input: Stack
; Output: None
; Modifies: EAX, ECX, EDX, ESI
; function SendKernelPanic {
; +EBP
Push EBP
; EBP = ESP
Mov EBP, ESP
    ; +All
    PushAD 
    ; Write the type
    ; AL = #Ds2Vs_KernelPanic
    Mov AL, DebugStub_Const_Ds2Vs_KernelPanic
    ; ComWriteAL()

    ; Write value
    ; EAX = [EBP + 8]
    Mov EAX, DWORD [EBP + 8]
    ; ComWriteEAX()

	; SendCoreDump()
    ; -All
    PopAD 
  ; -EBP
  Pop EBP
; }

; Input: Stack
; Output: None
; Modifies: EAX, ECX, EDX, ESI
; function SendSimpleLongNumber {
  ; +EBP
  Push EBP
  ; EBP = ESP
  Mov EBP, ESP
  ; +All
  PushAD 

  ; Write the type
  ; AL = #Ds2Vs_SimpleLongNumber
  Mov AL, DebugStub_Const_Ds2Vs_SimpleLongNumber
  ; ComWriteAL()

  ; Write value
  ; EAX = [EBP + 8]
  Mov EAX, DWORD [EBP + 8]
  ; ComWriteEAX()
  ; EAX = [EBP + 12]
  Mov EAX, DWORD [EBP + 12]
  ; ComWriteEAX()

  ; -All
  PopAD 
  ; -EBP
  Pop EBP
; }

; Input: Stack
; Output: None
; Modifies: EAX, ECX, EDX, ESI
; function SendComplexNumber {
  ; +EBP
  Push EBP
  ; EBP = ESP
  Mov EBP, ESP
  ; +All
  PushAD 

  ; Write the type
  ; AL = #Ds2Vs_ComplexNumber
  Mov AL, DebugStub_Const_Ds2Vs_ComplexNumber
  ; ComWriteAL()

  ; Write value
  ; EAX = [EBP+8]
  Mov EAX, DWORD [EBP + 8]
  ; ComWriteEAX()

  ; -All
  PopAD 
  ; -EBP
  Pop EBP
; }

; Input: Stack
; Output: None
; Modifies: EAX, ECX, EDX, ESI
; function SendComplexLongNumber {
  ; +EBP
  Push EBP
  ; EBP = ESP
  Mov EBP, ESP
  ; +All
  PushAD 

  ; Write the type
  ; AL = #Ds2Vs_ComplexLongNumber
  Mov AL, DebugStub_Const_Ds2Vs_ComplexLongNumber
  ; ComWriteAL()

  ; Write value
  ; EAX = [EBP+8]
  Mov EAX, DWORD [EBP + 8]
  ; ComWriteEAX()
  ; EAX = [EBP+12]
  Mov EAX, DWORD [EBP + 12]
  ; ComWriteEAX()

  ; -All
  PopAD 
  ; -EBP
  Pop EBP
; }

; Input: Stack
; Output: None
; Modifies: EAX, ECX, EDX, ESI
; function SendPtr {
    ; Write the type
    ; AL = #Ds2Vs_Pointer
    Mov AL, DebugStub_Const_Ds2Vs_Pointer
    ; ComWriteAL()

    ; pointer value
    ; ESI = [EBP+8]
    Mov ESI, DWORD [EBP + 8]
    ; ComWrite32()
; }

; Input: Stack
; Output: None
; Modifies: EAX, ECX, EDX, ESI
; function SendStackCorruptionOccurred {
    ; Write the type
    ; AL = #Ds2Vs_StackCorruptionOccurred
    Mov AL, DebugStub_Const_Ds2Vs_StackCorruptionOccurred
    ; ComWriteAL()

    ; pointer value
    ; ESI = @.CallerEIP
    Mov ESI, DebugStub_Var_CallerEIP
    ; ComWrite32()
; }

; Input: Stack
; Output: None
; Modifies: EAX, ECX, EDX, ESI
; function SendStackOverflowOccurred {
    ; Write the type
    ; AL = #Ds2Vs_StackOverflowOccurred
    Mov AL, DebugStub_Const_Ds2Vs_StackOverflowOccurred
    ; ComWriteAL()

    ; pointer value
    ; ESI = @.CallerEIP
    Mov ESI, DebugStub_Var_CallerEIP
    ; ComWrite32()
; }

; Input: None
; Output: None
; Modifies: EAX, ECX, EDX, ESI
; function SendInterruptOccurred {
    ; Write the type
	; +EAX
	Push EAX

		; AL = #Ds2Vs_InterruptOccurred
		Mov AL, DebugStub_Const_Ds2Vs_InterruptOccurred
		; ComWriteAL()

    ; -EAX
    Pop EAX
	; ComWriteEAX()
; }

; Input: Stack
; Output: None
; Modifies: EAX, ECX, EDX, ESI
; function SendNullReferenceOccurred {
    ; Write the type
    ; AL = #Ds2Vs_NullReferenceOccurred
    Mov AL, DebugStub_Const_Ds2Vs_NullReferenceOccurred
    ; ComWriteAL()

    ; pointer value
    ; ESI = @.CallerEIP
    Mov ESI, DebugStub_Var_CallerEIP
    ; ComWrite32()
; }

; Input: Stack
; Output: None
; Modifies: EAX, ECX, EDX, ESI
; function SendMessageBox {
    ; Write the type
    ; AL = #Ds2Vs_MessageBox
    Mov AL, DebugStub_Const_Ds2Vs_MessageBox
    ; ComWriteAL()

    ; Write Length
    ; ESI = EBP
    Mov ESI, EBP
    ; ESI += 12
    ; ECX = [ESI]
    Mov ECX, DWORD [ESI]
    ; ComWrite16()

    ; Address of string
    ; ESI = [EBP+8]
    Mov ESI, DWORD [EBP + 8]
; WriteChar:
    ; if ECX = 0 return
    ; ComWrite8()
    ; ECX--
    Dec ECX
    ; We are storing as 16 bits, but for now I will transmit 8 bits
    ; So we inc again to skip the 0
    ; ESI++
    Inc ESI
    ; goto WriteChar
; }

; function SendCoreDump {
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
    Mov EAX, DebugStub_Var_CallerEBP
    ; +EAX
    Push EAX
    ; EAX = @.CallerEIP
    Mov EAX, DebugStub_Var_CallerEIP
    ; +EAX
    Push EAX
    ; EAX = @.CallerESP
    Mov EAX, DebugStub_Var_CallerESP
    ; +EAX
    Push EAX
    ; ECX = 36
    Mov ECX, 0x24
    ; EAX = EBP
    Mov EAX, EBP
    ; while EAX != 0 {
        ; EAX -= 4
        ; +EAX
        Push EAX
        ; ECX += 4
        ; EAX = [EAX]
        Mov EAX, DWORD [EAX]
    ; }

    ; Send command
	; AL = #Ds2Vs_CoreDump
	Mov AL, DebugStub_Const_Ds2Vs_CoreDump
	; ComWriteAL()
    ; EAX = ECX
    Mov EAX, ECX
    ; ComWriteAX()
    ; while ECX != 0 {
        ; -EAX
        Pop EAX
        ; ComWriteEAX()
        ; ECX--
        Dec ECX
    ; }
; }
