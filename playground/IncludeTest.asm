; namespace DebugStub

; include Test.xs
; namespace DebugStub

; .v1 = 1
Mov DWORD [DebugStub_v1], 0x1
; .v1 = AL
Mov BYTE [DebugStub_v1], AL
; .v1 = EAX
Mov DWORD [DebugStub_v1], EAX
; .v1 = #const
Mov [DebugStub_v1], DebugStub_Const_const

; AH = 14
Mov AH, 0xE
; AL = 'H'
Mov AL, 0x48
; //! int 0x10
int 0x10
; AL = 'e'
Mov AL, 0x65
; //! int 0x10
int 0x10
; AL = 'l'
Mov AL, 0x6C
; //! int 0x10
int 0x10
; AL = 'l'
Mov AL, 0x6C
; //! int 0x10
int 0x10
; AL = 'o'
Mov AL, 0x6F
; //! int 0x10
int 0x10
; AL = ' '
Mov AL, 0x20
; //! int 0x10
int 0x10
; AL = 'f'
Mov AL, 0x66
; //! int 0x10
int 0x10
; AL = 'r'
Mov AL, 0x72
; //! int 0x10
int 0x10
; AL = 'o'
Mov AL, 0x6F
; //! int 0x10
int 0x10
; AL = 'm'
Mov AL, 0x6D
; //! int 0x10
int 0x10
; AL = ' '
Mov AL, 0x20
; //! int 0x10
int 0x10
; AL = 'X'
Mov AL, 0x58
; //! int 0x10
int 0x10
; AL = '#'
Mov AL, 0x23
; //! int 0x10
int 0x10

; .v = 4
Mov DWORD [DebugStub_v], 0x4
