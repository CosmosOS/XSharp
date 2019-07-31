using System.IO;
using NUnit.Framework;
using XSharp.x86.Assemblers;

namespace XSharp.Tests
{
    public class TokenMap_Should
    {
        private XSharp.Compiler mCompiler;
        private NASM mNASM;
        private Spruce.Tokens.Root mTokenMap;

        public TokenMap_Should()
        {
            mCompiler = new Compiler(TextWriter.Null);
            mNASM = new NASM(TextWriter.Null);
            mTokenMap = new Spruce.Tokens.Root();
            mTokenMap.AddEmitter(new Emitters.Namespace(mCompiler, mNASM));
            mTokenMap.AddEmitter(new Emitters.Comments(mCompiler, mNASM));
            mTokenMap.AddEmitter(new Emitters.Ports(mCompiler, mNASM));
            mTokenMap.AddEmitter(new Emitters.ZeroParamOps(mCompiler, mNASM)); // This should be above push/pop
            mTokenMap.AddEmitter(new Emitters.IncrementDecrement(mCompiler, mNASM)); // This should be above + operator
            mTokenMap.AddEmitter(new Emitters.PushPop(mCompiler, mNASM)); // This should be above + operator
            mTokenMap.AddEmitter(new Emitters.Assignments(mCompiler, mNASM));
            mTokenMap.AddEmitter(new Emitters.Test(mCompiler, mNASM));
            mTokenMap.AddEmitter(new Emitters.AllEmitters(mCompiler, mNASM));
        }

        [Test]
        public void Parse_Namespace_Definition_Keyword_And_Identifier()
        {
            const string xNamespaceDefinitionLine = "namespace DebugStub";
            const int xExpectedTokenCount = 2;

            var xCodePoints = mTokenMap.Parse(xNamespaceDefinitionLine);

            Assert.AreEqual(xExpectedTokenCount, xCodePoints.Count);
        }

        [Test]
        public void Parse_Function_Definition_Keyword_And_Identifier()
        {
            const string xFunctionDefinitionLine = "function DoAsmBreak {";
            const int xExpectedTokenCount = 3;

            var xCodePoints = mTokenMap.Parse(xFunctionDefinitionLine);

            Assert.AreEqual(xExpectedTokenCount, xCodePoints.Count);
        }

        [Test]
        public void Parse_Variable_Definition_Keyword_And_Identifier()
        {
            const string xVariableLine = "var AsmBreakEIP";
            const int xExpectedTokenCount = 2;

            var xCodePoints = mTokenMap.Parse(xVariableLine);

            Assert.AreEqual(xExpectedTokenCount, xCodePoints.Count);
        }

        [Test]
        public void Parse_Constant_Definition_Keyword_And_Identifier_And_Value()
        {
            const string xConstantLine = "const Tracing_Off = 0";
            const int xExpectedTokenCount = 4;

            var xCodePoints = mTokenMap.Parse(xConstantLine);

            Assert.AreEqual(xExpectedTokenCount, xCodePoints.Count);
        }

        [Test]
        public void Parse_Comment()
        {
            const string xCommentLine = "// Location where INT3 has been injected.";
            const int xExpectedTokenCount = 2;

            var xCodePoints = mTokenMap.Parse(xCommentLine);

            Assert.AreEqual(xExpectedTokenCount, xCodePoints.Count);
        }

        [Test]
        public void Parse_Assignment()
        {
            const string xAssignmentLune = "ESI = .CallerESP";
            const int xExpectedTokenCount = 3;

            var xCodePoints = mTokenMap.Parse(xAssignmentLune);

            Assert.AreEqual(xExpectedTokenCount, xCodePoints.Count);
        }

        [Test]
        public void Parse_Function_Call()
        {
            const string xFunctionCallLine = "ClearAsmBreak()";
            const int xExpectedTokenCount = 3;

            var xCodePoints = mTokenMap.Parse(xFunctionCallLine);

            Assert.AreEqual(xExpectedTokenCount, xCodePoints.Count);
        }

        [Test]
        public void Parse_Push()
        {
            const string xPushLine = "+EAX";
            const int xExpectedTokenCount = 2;

            var xCodePoints = mTokenMap.Parse(xPushLine);

            Assert.AreEqual(xExpectedTokenCount, xCodePoints.Count);
        }

        [Test]
        public void Parse_Pop()
        {
            const string xPopLine = "-EAX";
            const int xExpectedTokenCount = 2;

            var xCodePoints = mTokenMap.Parse(xPopLine);

            Assert.AreEqual(xExpectedTokenCount, xCodePoints.Count);
        }

        [Test]
        public void Parse_Conditional()
        {
            const string xConditionalLine = "if AL = #Vs2Ds_Noop return";
            const int xExpectedTokenCount = 3;

            var xCodePoints = mTokenMap.Parse(xConditionalLine);

            Assert.AreEqual(xExpectedTokenCount, xCodePoints.Count);
        }

        [Test]
        public void Parse_Return()
        {
            const string xReturnLine = "return";
            const int xExpectedTokenCount = 1;

            var xCodePoints = mTokenMap.Parse(xReturnLine);

            Assert.AreEqual(xExpectedTokenCount, xCodePoints.Count);
        }
    }
}
