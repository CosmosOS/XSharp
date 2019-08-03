



%ifndef Exclude_IOPort_Based_Serial



DebugStub_WriteRegister:
  Push EDX
    Add DX, 0x3F8
    Out DX, AL
  Pop EDX

DebugStub_ReadRegister:
  Push EDX
    Add DX, 0x3F8
    In AL, DX
  Pop EDX

%endif
