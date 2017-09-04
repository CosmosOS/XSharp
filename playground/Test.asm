; namespace DebugStub

; +#SomeConst
Push DebugStub_Const_SomeConst

; -.testVar
Pop DebugStub_Var_testVar
; -@.testVar
Pop [DebugStub_Var_testVar]

; EAX = [EBX]
Mov EAX, [EBX]
