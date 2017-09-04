; namespace DebugStub

; function SendRegisters {
    ; Send the actual started signal
    ; AL = #Ds2Vs_Registers
    ; ComWriteAL()

    ; ESI = .PushAllPtr
    ; ECX = 32
    ; ComWriteX()

    ; ESI = @.CallerESP
    ; ComWrite32()

    ; ESI = @.CallerEIP
    ; ComWrite32()
; }

; function SendFrame {
    ; AL = #Ds2Vs_Frame
    ; ComWriteAL()

    ; EAX = 32
    ; ComWriteAX()

    ; ESI = .CallerEBP
    ; Dont transmit EIP or old EBP
    ; ESI += 8
    ; ECX = 32
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

  ; +All
  PushAD 
    ; ComWriteAL()
  ; -All
  PopAD 

  ; +All
  PushAD 
    ; EAX = ECX
    ; ComWriteEAX()
  ; -All
  PopAD 

  ; now ECX contains size of data (count)
    ; ESI contains address
    ; while ECX != 0 {
        ; ComWrite8()
        ; ECX--
    ; }
; }

; function SendStack {
    ; AL = #Ds2Vs_Stack
    ; ComWriteAL()

    ; Send size of bytes
    ; ESI = .CallerESP
    ; EAX = .CallerEBP
    ; EAX -= ESI
    ; ComWriteAX()

    ; Send actual bytes
    ; Need to reload ESI, WriteAXToCompPort modifies it
    ; ESI = .CallerESP
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
    ; ComWriteAL()

    ; ESI = .CallerEBP

    ; offset relative to ebp
    ; size of data to send
    ; ComReadEAX()
    ; ESI += EAX
    ; ComReadEAX()
    ; ECX = EAX

    ; now ECX contains size of data (count)
    ; ESI contains relative to EBP

    ; while ECX != 0 {
        ; ComWrite8()
        ; ECX--
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
    ; ComWriteAL()

    ; ComReadEAX()
    ; ESI = EAX
    ; ComReadEAX()
    ; ECX = EAX

    ; now ECX contains size of data (count)
    ; ESI contains address
    ; while ECX != 0 {
        ; ComWrite8()
        ; ECX--
    ; }

; Exit:
    ; -All
    PopAD 
; }

; Modifies: EAX, ESI
; function SendTrace {
    ; AL = #Ds2Vs_BreakPoint
    ; If we are running, its a tracepoint, not a breakpoint.
    ; In future, maybe separate these into 2 methods
    ; if dword .DebugStatus = #Status_Run {
        ; AL = #Ds2Vs_TracePoint
    ; }
    ; ComWriteAL()

    ; Send Calling EIP.
    ; ESI = @.CallerEIP
    ; ComWrite32()
; }

; Input: Stack
; Output: None
; Modifies: EAX, ECX, EDX, ESI
; function SendText {
; +EBP
; EBP = ESP
    ; +All
    PushAD 
    ; Write the type
    ; AL = #Ds2Vs_Message
    ; ComWriteAL()

    ; Write Length
    ; ESI = EBP
    ; ESI += 12
    ; ECX = [ESI]
    ; ComWrite16()

    ; Address of string
    ; ESI = [EBP + 8]
; WriteChar:
    ; if ECX = 0 goto Finalize
    ; ComWrite8()
    ; ECX--
    ; We are storing as 16 bits, but for now I will transmit 8 bits
    ; So we inc again to skip the 0
    ; ESI++
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
; }

; Input: Stack
; Output: None
; Modifies: EAX, ECX, EDX, ESI
; function SendSimpleNumber {
; +EBP
; EBP = ESP
    ; +All
    PushAD 
    ; Write the type
    ; AL = #Ds2Vs_SimpleNumber
    ; ComWriteAL()

    ; Write value
    ; EAX = [EBP + 8]
    ; ComWriteEAX()

    ; -All
    PopAD 
  ; -EBP
; }

; Input: Stack
; Output: None
; Modifies: EAX, ECX, EDX, ESI
; function SendKernelPanic {
; +EBP
; EBP = ESP
    ; +All
    PushAD 
    ; Write the type
    ; AL = #Ds2Vs_KernelPanic
    ; ComWriteAL()

    ; Write value
    ; EAX = [EBP + 8]
    ; ComWriteEAX()

	; SendCoreDump()
    ; -All
    PopAD 
  ; -EBP
; }

; Input: Stack
; Output: None
; Modifies: EAX, ECX, EDX, ESI
; function SendSimpleLongNumber {
  ; +EBP
  ; EBP = ESP
  ; +All
  PushAD 

  ; Write the type
  ; AL = #Ds2Vs_SimpleLongNumber
  ; ComWriteAL()

  ; Write value
  ; EAX = [EBP + 8]
  ; ComWriteEAX()
  ; EAX = [EBP + 12]
  ; ComWriteEAX()

  ; -All
  PopAD 
  ; -EBP
; }

; Input: Stack
; Output: None
; Modifies: EAX, ECX, EDX, ESI
; function SendComplexNumber {
  ; +EBP
  ; EBP = ESP
  ; +All
  PushAD 

  ; Write the type
  ; AL = #Ds2Vs_ComplexNumber
  ; ComWriteAL()

  ; Write value
  ; EAX = [EBP+8]
  ; ComWriteEAX()

  ; -All
  PopAD 
  ; -EBP
; }

; Input: Stack
; Output: None
; Modifies: EAX, ECX, EDX, ESI
; function SendComplexLongNumber {
  ; +EBP
  ; EBP = ESP
  ; +All
  PushAD 

  ; Write the type
  ; AL = #Ds2Vs_ComplexLongNumber
  ; ComWriteAL()

  ; Write value
  ; EAX = [EBP+8]
  ; ComWriteEAX()
  ; EAX = [EBP+12]
  ; ComWriteEAX()

  ; -All
  PopAD 
  ; -EBP
; }

; Input: Stack
; Output: None
; Modifies: EAX, ECX, EDX, ESI
; function SendPtr {
    ; Write the type
    ; AL = #Ds2Vs_Pointer
    ; ComWriteAL()

    ; pointer value
    ; ESI = [EBP+8]
    ; ComWrite32()
; }

; Input: Stack
; Output: None
; Modifies: EAX, ECX, EDX, ESI
; function SendStackCorruptionOccurred {
    ; Write the type
    ; AL = #Ds2Vs_StackCorruptionOccurred
    ; ComWriteAL()

    ; pointer value
    ; ESI = @.CallerEIP
    ; ComWrite32()
; }

; Input: Stack
; Output: None
; Modifies: EAX, ECX, EDX, ESI
; function SendStackOverflowOccurred {
    ; Write the type
    ; AL = #Ds2Vs_StackOverflowOccurred
    ; ComWriteAL()

    ; pointer value
    ; ESI = @.CallerEIP
    ; ComWrite32()
; }

; Input: None
; Output: None
; Modifies: EAX, ECX, EDX, ESI
; function SendInterruptOccurred {
    ; Write the type
	; +EAX

		; AL = #Ds2Vs_InterruptOccurred
		; ComWriteAL()

    ; -EAX
	; ComWriteEAX()
; }

; Input: Stack
; Output: None
; Modifies: EAX, ECX, EDX, ESI
; function SendNullReferenceOccurred {
    ; Write the type
    ; AL = #Ds2Vs_NullReferenceOccurred
    ; ComWriteAL()

    ; pointer value
    ; ESI = @.CallerEIP
    ; ComWrite32()
; }

; Input: Stack
; Output: None
; Modifies: EAX, ECX, EDX, ESI
; function SendMessageBox {
    ; Write the type
    ; AL = #Ds2Vs_MessageBox
    ; ComWriteAL()

    ; Write Length
    ; ESI = EBP
    ; ESI += 12
    ; ECX = [ESI]
    ; ComWrite16()

    ; Address of string
    ; ESI = [EBP+8]
; WriteChar:
    ; if ECX = 0 return
    ; ComWrite8()
    ; ECX--
    ; We are storing as 16 bits, but for now I will transmit 8 bits
    ; So we inc again to skip the 0
    ; ESI++
    ; goto WriteChar
; }

; function SendCoreDump {
    ; +EAX
    ; +EBX
    ; +ECX
    ; +EDX
    ; +EDI
    ; +ESI
    ; EAX = @.CallerEBP
    ; +EAX
    ; EAX = @.CallerEIP
    ; +EAX
    ; EAX = @.CallerESP
    ; +EAX
    ; ECX = 36
    ; EAX = EBP
    ; while EAX != 0 {
        ; EAX -= 4
        ; +EAX
        ; ECX += 4
        ; EAX = [EAX]
    ; }

    ; Send command
	; AL = #Ds2Vs_CoreDump
	; ComWriteAL()
    ; EAX = ECX
    ; ComWriteAX()
    ; while ECX != 0 {
        ; -EAX
        ; ComWriteEAX()
        ; ECX--
    ; }
; }
