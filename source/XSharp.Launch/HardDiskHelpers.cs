using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace XSharp.Launch
{
    internal static class HardDiskHelpers
    {
        public enum HardDiskType
        {
            Vmdk,
            Vhdx
        }

        private static Dictionary<HardDiskType, string> HardDiskFileExtensions = new Dictionary<HardDiskType, string>()
        {
            { HardDiskType.Vmdk, ".vmdk" },
            { HardDiskType.Vmdk, ".vhdx" }
        };

        public static string CreateDiskOnRequestedPathOrDefault(string aPath, string aDefaultPath, HardDiskType aHardDiskType)
        {
            if (String.IsNullOrWhiteSpace(aDefaultPath))
            {
                throw new ArgumentException(nameof(aDefaultPath) + " is null or empty!");
            }

            if (String.IsNullOrWhiteSpace(aPath))
            {
                CreateDisk(aDefaultPath, aHardDiskType);
                return aDefaultPath;
            }
            else
            {
                if (!File.Exists(aPath))
                {
                    CreateDisk(aPath, aHardDiskType);
                }

                return aPath;
            }
        }

        private static void CreateDisk(string aPath, HardDiskType aHardDiskType)
        {
            if (String.IsNullOrWhiteSpace(aPath))
            {
                throw new ArgumentException("Path is null or empty!");
            }

            var xHardDiskExtension = HardDiskFileExtensions[aHardDiskType];
            var xHardDiskResource = $"Resources.FileSystem" + xHardDiskExtension;

            using (var xStream = Assembly.GetExecutingAssembly().GetManifestResourceStream(typeof(HardDiskHelpers), xHardDiskResource))
            {
                using (var xFile = File.Create(aPath))
                {
                    xStream.CopyTo(xFile);
                }
            }
        }
    }
}
