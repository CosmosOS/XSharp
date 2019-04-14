; namespace DebugStub

; handles the reading and writing to serial (16550A) registers

; optionally exclude this serial version


; //! %ifndef Exclude_IOPort_Based_Serial
%ifndef Exclude_IOPort_Based_Serial

; mComPortAddresses = 0x3F8, 0x2F8, 0x3E8, 0x2E8;
; Currently hardcoded to COM1.
; var ComAddr = $03F8

; writes to a UART register
; uses:
; DX -> Port offset
; AL -> value to write
; Modifies:
; DX

; function WriteRegister {
DebugStub_WriteRegister:
  ; +EDX
  Push EDX
    ; DX += $03F8
    ; Port[DX] = AL
    Out DX, AL
  ; -EDX
  Pop EDX
; }

; reads from a UART register
; uses:
; DX -> Port offset
; Modifies:
; AL -> value read
; function ReadRegister {
DebugStub_ReadRegister:
  ; +EDX
  Push EDX
    ; DX += $03F8
    ; AL = Port[DX]
    In AL, DX
  ; -EDX
  Pop EDX
; }

; //! %endif
%endif
