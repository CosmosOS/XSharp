namespace XSharp.Assembler.x86
{
    public class ExternalLabel: Instruction
    {
        public ExternalLabel(string aName):base()
        {
            Name = aName;
        }

        public string Name
        {
            get;
            set;
        }

        public override void WriteText( XSharp.Assembler.Assembler aAssembler, System.IO.TextWriter aOutput )
        {
            aOutput.Write("extern ");
            aOutput.Write(Name);
        }
    }
}