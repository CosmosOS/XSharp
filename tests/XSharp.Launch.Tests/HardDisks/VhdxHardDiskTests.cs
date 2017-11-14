using Xunit;

using XSharp.Launch.HardDisks;

namespace XSharp.Launch.Tests
{
    public class VhdxHardDiskTests
    {
        [Fact]
        public void CreateDiskWithSampleContent()
        {
            System.IO.Directory.CreateDirectory("TestDir");
            System.IO.File.Create(@"TestDir\Sample.vhdx").Dispose();
            var xHardDisk = new VhdxHardDisk(@"TestDir\Sample.vhdx", 20 * 1024 * 1024);

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
