namespace XSharp.Assembler
{
    public class IfNotDefined: Instruction, IIfNotDefined {
        public string Symbol {
            get;
            set;
        }

        public override void WriteText(XSharp.Assembler.Assembler aAssembler, System.IO.TextWriter aOutput)
        {
            aOutput.Write(this.GetAsText());
        }
    }
}