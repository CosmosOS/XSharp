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

            var xBochsHost = new Bochs(xLaunchSettings);

            xBochsHost.Start();

            var xProcessCount = Process.GetProcessesByName(xProcessName).Length;
            Assert.That(xProcessCount, Is.Not.Zero);

            xBochsHost.Stop();
            Assert.That(Process.GetProcessesByName(xProcessName).Length, Is.EqualTo(xProcessCount - 1));
        }
    }
}
