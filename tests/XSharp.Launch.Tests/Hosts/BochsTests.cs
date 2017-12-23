using System.Diagnostics;
using System.IO;

using NUnit.Framework;

using XSharp.Launch.Hosts.Bochs;

namespace XSharp.Launch.Tests.Hosts
{
    [TestFixture(TestOf = typeof(BochsHost))]
    public class BochsTests
    {
        private string TestDir = TestUtilities.NewTestDir();

        [TestCase(false)]
        [TestCase(true)]
        public void LaunchBochs(bool aUseDebugVersion)
        {
            var xProcessName = aUseDebugVersion ? "bochsdbg" : "bochs";

            var xLaunchSettings = new BochsLaunchSettings()
            {
                ConfigurationFile = Path.Combine(TestDir, "Bochs.bxrc"),
                BochsDirectory = null,
                UseDebugVersion = aUseDebugVersion
            };

            var xBochsHost = new BochsHost(xLaunchSettings);

            xBochsHost.Start();

            var xProcessCount = Process.GetProcessesByName(xProcessName).Length;
            Assert.That(xProcessCount, Is.Not.Zero);

            xBochsHost.Kill();
            Assert.That(Process.GetProcessesByName(xProcessName).Length, Is.EqualTo(xProcessCount - 1));
        }
    }
}
