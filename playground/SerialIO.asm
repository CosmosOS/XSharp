; namespace DebugStub




; //! %ifndef Exclude_IOPort_Based_Serial
%ifndef Exclude_IOPort_Based_Serial

; var ComAddr = $03F8


; function WriteRegister {
DebugStub_WriteRegister:
  ; +EDX
  Push EDX
    ; DX += $03F8
    Add DX, 0x3F8
    ; Port[DX] = AL
    Out DX, AL
  ; -EDX
  Pop EDX
; }

; function ReadRegister {
DebugStub_ReadRegister:
  ; +EDX
  Push EDX
    ; DX += $03F8
    Add DX, 0x3F8
    ; AL = Port[DX]
    In AL, DX
  ; -EDX
  Pop EDX
; }

; //! %endif
%endif
