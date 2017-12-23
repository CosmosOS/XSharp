using System;
using System.IO;

namespace XSharp.Launch.Tests
{
    internal static class TestUtilities
    {
        private static string TestRoot = "TestDir";

        public static string NewTestDir()
        {
            string xFolderName = Guid.NewGuid().ToString();
            xFolderName = Path.Combine(TestRoot, xFolderName);

            Directory.CreateDirectory(xFolderName);

            return xFolderName;
        }
    }
}
