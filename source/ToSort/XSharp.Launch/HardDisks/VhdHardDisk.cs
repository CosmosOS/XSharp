using System;
using System.IO;

using DiscUtils;
using DiscUtils.Fat;
using DiscUtils.Partitions;
using DiscUtils.Streams;
using DiscUtils.Vhd;

namespace XSharp.Launch.HardDisks
{
    public class VhdHardDisk : VirtualHardDiskBase
    {
        private Disk mDisk;
        private FatFileSystem mFileSystem;

        public override VirtualDisk VirtualDisk => mDisk;
        public override DiscFileSystem FileSystem => mFileSystem;

        public VhdHardDisk(string aDiskPath, uint aDiskSize)
            : base (aDiskPath, aDiskSize)
        {
        }

        public override void Initialize(Action<DiscFileSystem> aInitializeHardDiskContents)
        {
            if (IsInitialized)
            {
                throw new Exception("Disk is already initialized!");
            }

            using (var xStream = File.Create(DiskPath))
            {
                mDisk = Disk.InitializeDynamic(xStream, Ownership.Dispose, DiskSize);
                BiosPartitionTable.Initialize(mDisk, WellKnownPartitionType.WindowsFat);

                mFileSystem = FatFileSystem.FormatPartition(mDisk, 0, null);
                aInitializeHardDiskContents(mFileSystem);
            }
        }
    }
}
