using System;

using XSharp.Assembler.x86.SSE;
using XSharp.Assembler.x86.x87;

namespace XSharp
{
  using static XSRegisters;

  partial class XS
  {
    public static class SSE
    {
      public static void SSEInit()
      {
         XS.Comment("BEGIN - SSE Init");

         // CR4[bit 9]=1, CR4[bit 10]=1, CR0[bit 2]=0, CR0[bit 1]=1

         XS.Set(RAX, CR4);
         XS.Or(RAX, 0x100);
         XS.Set(CR4, RAX);
         XS.Set(RAX, CR4);
         XS.Or(RAX, 0x200);
         XS.Set(CR4, RAX);
         XS.Set(RAX, CR0);
         XS.And(RAX, 0xfffffffd);
         XS.Set(CR0, RAX);
         XS.Set(RAX, CR0);

         XS.And(RAX, 1);
         XS.Set(CR0, RAX);
         XS.Comment("END - SSE Init");
     }

      public static void AddSS(RegisterXMM destination, RegisterXMM source)
      {
        DoDestinationSource<AddSS>(destination, source);
      }

      public static void MulSS(RegisterXMM destination, RegisterXMM source)
      {
        DoDestinationSource<MulSS>(destination, source);
      }

      public static void SubSS(RegisterXMM destination, RegisterXMM source)
      {
        DoDestinationSource<SubSS>(destination, source);
      }

      public static void XorPS(RegisterXMM destination, RegisterXMM source)
      {
        DoDestinationSource<XorPS>(destination, source);
      }

      public static void CompareSS(RegisterXMM destination, RegisterXMM source, ComparePseudoOpcodes comparision)
      {
         new CompareSS()
         {
            DestinationReg = destination,
            SourceReg = source,
            pseudoOpcode = (byte) comparision
          };
      }

      public static void ConvertSI2SS(RegisterXMM destination, Register64 source, bool sourceIsIndirect = false)
      {
        new ConvertSI2SS()
        {
          DestinationReg = destination,
          SourceReg = source,
          SourceIsIndirect = sourceIsIndirect
        };
      }

      public static void MoveSS(RegisterXMM destination, RegisterXMM source)
      {
        DoDestinationSource<MoveSS>(destination, source);
      }

      public static void MoveSS(RegisterXMM destination, Register64 source, bool sourceIsIndirect = false)
      {
        new MoveSS()
        {
          DestinationReg = destination,
          SourceReg = source,
          SourceIsIndirect = sourceIsIndirect
        };
      }

      public static void MoveSS(Register64 destination, RegisterXMM source, bool destinationIsIndirect = false)
      {
        new MoveSS()
        {
          DestinationReg = destination,
          DestinationIsIndirect = destinationIsIndirect,
          SourceReg = source
        };
      }

      public static void MoveSS(RegisterXMM destination, String sourceLabel, bool destinationIsIndirect = false, int? destinationDisplacement = null, bool sourceIsIndirect = false, int? sourceDisplacement = null)
      {
          DoDestinationSource<MoveSS>(destination, sourceLabel, destinationIsIndirect, destinationDisplacement, sourceIsIndirect, sourceDisplacement);
      }

      public static void MoveUPS(Register64 destination, RegisterXMM source, bool destinationIsIndirect = false, int? destinationDisplacement = null, bool sourceIsIndirect = false, int? sourceDisplacement = null)
      {
        DoDestinationSource<MoveUPS>(destination, source, destinationIsIndirect, destinationDisplacement, sourceIsIndirect, sourceDisplacement);
      }

      public static void MoveDQA(RegisterXMM destination, Register64 source, bool destinationIsIndirect = false, int? destinationDisplacement = null, bool sourceIsIndirect = false, int? sourceDisplacement = null)
      {
         DoDestinationSource<MoveDQA>(destination, source, destinationIsIndirect, destinationDisplacement, sourceIsIndirect, sourceDisplacement);
      }

      public static void MoveDQA(Register64 destination, RegisterXMM source, bool destinationIsIndirect = false, int? destinationDisplacement = null, bool sourceIsIndirect = false, int? sourceDisplacement = null)
      {
                DoDestinationSource<MoveDQA>(destination, source, destinationIsIndirect, destinationDisplacement, sourceIsIndirect, sourceDisplacement);
      }

      public static void MoveDQU(RegisterXMM destination, Register64 source, bool destinationIsIndirect = false, int? destinationDisplacement = null, bool sourceIsIndirect = false, int? sourceDisplacement = null)
      {
        DoDestinationSource<MoveDQU>(destination, source, destinationIsIndirect, destinationDisplacement, sourceIsIndirect, sourceDisplacement);
      }

      public static void MoveDQU(Register64 destination, RegisterXMM source, bool destinationIsIndirect = false, int? destinationDisplacement = null, bool sourceIsIndirect = false, int? sourceDisplacement = null)
      {
          DoDestinationSource<MoveDQU>(destination, source, destinationIsIndirect, destinationDisplacement, sourceIsIndirect, sourceDisplacement);
      }

      public static void MoveAPS(Register64 destination, RegisterXMM source, bool destinationIsIndirect = false, int? destinationDisplacement = null, bool sourceIsIndirect = false, int? sourceDisplacement = null)
      {
          DoDestinationSource<MoveAPS>(destination, source, destinationIsIndirect, destinationDisplacement, sourceIsIndirect, sourceDisplacement);
      }

      public static void MoveAPS(RegisterXMM destination, Register64 source, bool destinationIsIndirect = false, int? destinationDisplacement = null, bool sourceIsIndirect = false, int? sourceDisplacement = null)
      {
          DoDestinationSource<MoveAPS>(destination, source, destinationIsIndirect, destinationDisplacement, sourceIsIndirect, sourceDisplacement);
      }

      public static void ConvertSS2SIAndTruncate(Register64 destination, RegisterXMM source)
      {
        new ConvertSS2SIAndTruncate
        {
          DestinationReg = destination,
          SourceReg = source
        };
      }

      public static void DivPS(RegisterXMM destination, RegisterXMM source)
      {
        new DivPS
        {
          DestinationReg = destination,
          SourceReg = source
        };
      }

      public static void DivSS(RegisterXMM destination, RegisterXMM source)
      {
        new DivSS
        {
          DestinationReg = destination,
          SourceReg = source
        };
      }

      public static void FXSave(Register64 destination, bool isIndirect)
      {
        new FXSave
        {
          DestinationReg = destination,
          DestinationIsIndirect = isIndirect
        };
      }

      public static void FXRestore(Register64 destination, bool isIndirect)
      {
        new FXStore()
        {
          DestinationReg = destination,
          DestinationIsIndirect = isIndirect
        };
      }

      public static void Shufps(RegisterXMM destination, RegisterXMM source, int bitmask)
      {
        new Shufps()
        {
          DestinationReg = destination,
          SourceReg = source,
          pseudoOpcode = (byte)bitmask
        };
      }
    }
  }
}
