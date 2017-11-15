using System.Diagnostics;
using System.IO;

using NUnit.Framework;

using XSharp.Launch.Hosts.Bochs;

namespace XSharp.Launch.Tests.Hosts
{
    [TestFixture(TestOf = typeof(Bochs))]
    public class BochsTests
    {
        private string TestDir = TestUtilities.NewTestDir();

        [Test]
        public void LaunchBochs()
        {
            var xLaunchSettings = new BochsLaunchSettings()
            {
                ConfigurationFile = Path.Combine(TestDir, "Bochs.bxrc"),
                BochsDirectory = null,
                IsoFile = "",
                HardDiskFile = ""
            };

            var xBochsHost = new Bochs(xLaunchSettings);

            xBochsHost.Start();

            var xProcessCount = Process.GetProcessesByName("bochs").Length;
            Assert.That(xProcessCount, Is.Not.Zero);

            xBochsHost.Stop();
            Assert.That(Process.GetProcessesByName("bochs").Length, Is.EqualTo(xProcessCount - 1));
        }

        [Test]
        public void LaunchBochsDebugVersion()
        {
            var xLaunchSettings = new BochsLaunchSettings()
            {
                ConfigurationFile = Path.Combine(TestDir, "BochsDebug.bxrc"),
                BochsDirectory = null,
                IsoFile = "",
                HardDiskFile = "",
                UseDebugVersion = true
            };

            var xBochsHost = new Bochs(xLaunchSettings);

            xBochsHost.Start();

            var xProcessCount = Process.GetProcessesByName("bochsdbg").Length;
            Assert.That(xProcessCount, Is.Not.Zero);

            xBochsHost.Stop();
            Assert.That(Process.GetProcessesByName("bochsdbg").Length, Is.EqualTo(xProcessCount - 1));
        }
    }
}
