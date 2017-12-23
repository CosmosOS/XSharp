using System.IO;

using NUnit.Framework;

using XSharp.Launch.HardDisks;

namespace XSharp.Launch.Tests.HardDisks
{
    [TestFixture(TestOf = typeof(VhdxHardDisk))]
    internal class VhdxHardDiskTests
    {
        private string TestDir = TestUtilities.NewTestDir();

        [Test]
        public void CreateDiskWithSampleContent()
        {
            var xVhdxFile = Path.Combine(TestDir, "Sample.vhdx");
            File.Create(xVhdxFile).Dispose();

            var xHardDisk = new VhdxHardDisk(xVhdxFile, 20 * 1024 * 1024);

            Assert.False(xHardDisk.IsInitialized);

            xHardDisk.InitializeWithSampleContent();

            Assert.True(xHardDisk.IsInitialized);

            //Assert.True(xHardDisk.FileSystem.DirectoryExists(@"DirTesting"));
            Assert.True(xHardDisk.FileSystem.DirectoryExists(@"test"));
            //Assert.True(xHardDisk.FileSystem.DirectoryExists(@"test\DirInTest"));

            //Assert.True(xHardDisk.FileSystem.FileExists(@"test\DirInTest\Readme.txt"));
            Assert.True(xHardDisk.FileSystem.FileExists(@"Kudzu.txt"));
            Assert.True(xHardDisk.FileSystem.FileExists(@"Root.txt"));
        }
    }
}
