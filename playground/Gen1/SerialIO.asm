; Generated at 4/14/2019 1:59:47 AM

DebugStub_ComAddr dd 1016

%ifndef Exclude_IOPort_Based_Serial

DebugStub_WriteRegister:
push dword EDX
add DX, 0x3F8
out DX, AL
pop dword EDX

DebugStub_WriteRegister_Exit:
mov dword [INTs_LastKnownAddress], DebugStub_WriteRegister_Exit
Ret


DebugStub_ReadRegister:
push dword EDX
add DX, 0x3F8
in byte AL, DX
pop dword EDX

DebugStub_ReadRegister_Exit:
mov dword [INTs_LastKnownAddress], DebugStub_ReadRegister_Exit
Ret

%endif
