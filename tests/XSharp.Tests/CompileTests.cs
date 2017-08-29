using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using NUnit.Framework;

namespace XSharp.Tests
{
    [TestFixture]
    public class CompileTests
    {
        private static string mAssemblyDir = Path.GetDirectoryName(typeof(CompileTests).Assembly.Location);
        private static bool IgnoreCase = false;
        private static bool Trim = false;
        private static bool SkipLineComments = false;

        private static IEnumerable<string> GetXSharpInput()
        {
            var xInputDir = new DirectoryInfo(Path.Combine(mAssemblyDir, "Input"));
            return xInputDir.GetFiles("*.xs").Select(f => f.FullName);
        }

        [TestCaseSource("GetXSharpInput")]
        public void TestCompilation(string aPath)
        {
            var xExpectedOutputFile = Path.Combine(Path.GetDirectoryName(aPath), "..",
                "ExpectedOutput", Path.ChangeExtension(Path.GetFileName(aPath), ".asm"));

            using (var xOutputStream = new MemoryStream())
            {
                var xWriter = new StreamWriter(xOutputStream);
                var xCompiler = new Compiler(xWriter);

                using (var xReader = new StreamReader(File.OpenRead(aPath)))
                {
                    xCompiler.Emit(xReader);
                }

                xWriter.Flush();

                xOutputStream.Position = 0;

                using (var xExpectedOutputReader = new StreamReader(File.OpenRead(xExpectedOutputFile)))
                {
                    using (var xActualOutputReader = new StreamReader(xOutputStream))
                    {
                        while (!xActualOutputReader.EndOfStream)
                        {
                            var xActualLine = xActualOutputReader.ReadLine();
                            var xExpectedLine = xExpectedOutputReader.ReadLine();

                            if (SkipLineComments
                                && xActualLine.Trim().StartsWith(';')
                                && xExpectedLine.StartsWith(';'))
                            {
                                continue;
                            }

                            if (IgnoreCase)
                            {
                                xActualLine = xActualLine.ToLower();
                                xExpectedLine = xExpectedLine.ToLower();
                            }

                            if (Trim)
                            {
                                xActualLine = xActualLine.Trim();
                                xExpectedLine = xExpectedLine.Trim();
                            }

                            Assert.That(xActualLine, Is.EqualTo(xExpectedLine));
                        }
                    }
                }
            }
        }
    }
}
