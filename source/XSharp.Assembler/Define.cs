namespace XSharp.Assembler
{
    [XSharp.Assembler.OpCode("%define")]
    public class Define: Instruction, IDefine {
        public string Symbol {
            get;
            set;
        }

        public Define(string aSymbol) {
            Symbol = aSymbol;
        }

        public override void WriteText(XSharp.Assembler.Assembler aAssembler, System.IO.TextWriter aOutput)
        {
            aOutput.Write(this.GetAsText());
        }
    }
}