# X# (X Sharp)
X# is a HLA (High Level Assembler) for X86/X64 assembly. In the future other flavors for ARM and other processors are planned.

X# is integrated into Visual Studio and we are working on support for Visual Studio Code as well. X# can also be used using simple text editors such as notepad.

X# creates NASM style assembly ready to assemble with [NASM](http://www.nasm.us/).

# Status
Currently X# is used by the [C# Open Source Managed Operating System (COSMOS)](http://www.goCosmos.org) and parts of it are bound to Cosmos. We are in the process of and nearly finished separating out X# to allow it to operate as a stand alone project to allow users to make their own custom creations using X#.

# Sample

```
namespace DebugStub

var .DebugWaitMsg = 'Waiting for debugger connection...'

! %ifndef Exclude_Memory_Based_Console

const VidBase = $B8000

function Cls {
    ESI = #VidBase

	// End of Video Area
	// VidBase + 25 * 80 * 2 = B8FA0
	while ESI < $B8FA0 {
		// Text
		ESI[0] = $00
		ESI++

		// Colour
		ESI[0] = $0A
		ESI++
	}
}

function DisplayWaitMsg {
	ESI = @..DebugWaitMsg

    EDI = #VidBase
    // 10 lines down, 20 cols in (10 * 80 + 20) * 2)
    EDI + 1640

    // Read and copy string till 0 terminator
    while byte ESI[0] != 0 {
		AL = ESI[0]
		EDI[0] = AL
		ESI++
		EDI + 2
	}
}

! %endif
```

# Resulting NASM Output

```
; Generated at 14-6-2016 12:37:38


	DebugWaitMsg db 87, 97, 105, 116, 105, 110, 103, 32, 102, 111, 114, 32, 100, 101, 98, 117, 103, 103, 101, 114, 32, 99, 111, 110, 110, 101, 99, 116, 105, 111, 110, 46, 46, 46, 0

			%ifndef Exclude_Memory_Based_Console
			DebugStub_Const_VidBase equ 753664

		DebugStub_Cls:
			mov dword ESI, DebugStub_Const_VidBase

		DebugStub_Cls_Block1_Begin:
			cmp dword ESI, 0xB8FA0
			JNB near DebugStub_Cls_Block1_End
			mov dword [ESI], 0x0
			inc dword ESI
			mov dword [ESI], 0xA
			inc dword ESI
			Jmp DebugStub_Cls_Block1_Begin

		DebugStub_Cls_Block1_End:

		DebugStub_Cls_Exit:
			mov dword [static_field__Cosmos_Core_INTs_mLastKnownAddress], DebugStub_Cls_Exit
			Ret


		DebugStub_DisplayWaitMsg:
			mov dword ESI, DebugWaitMsg
			mov dword EDI, DebugStub_Const_VidBase
			add dword EDI, 0x668

		DebugStub_DisplayWaitMsg_Block1_Begin:
			cmp byte [ESI], 0x0
			JE near DebugStub_DisplayWaitMsg_Block1_End
			mov byte AL, [ESI]
			mov byte [EDI], AL
			inc dword ESI
			add dword EDI, 0x2
			Jmp DebugStub_DisplayWaitMsg_Block1_Begin

		DebugStub_DisplayWaitMsg_Block1_End:

		DebugStub_DisplayWaitMsg_Exit:
			mov dword [static_field__Cosmos_Core_INTs_mLastKnownAddress], DebugStub_DisplayWaitMsg_Exit
			Ret

			%endif
```
