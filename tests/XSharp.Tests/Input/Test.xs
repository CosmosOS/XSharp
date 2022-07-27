namespace DebugStub
var DebugBPs dword[256]
// Temp Test Area
    //! nop
    AH = 0
    AX = 0
	EAX = 0
    NOP
    return
	EAX = $FFFF
	EAX = $FFFFFFFF
	+All
	-All

// Modifies: AL, DX (ComReadAL)
// Returns: AL
//END
