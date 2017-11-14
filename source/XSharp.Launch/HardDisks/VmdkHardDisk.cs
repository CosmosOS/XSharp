using System;
using System.IO;

using DiscUtils;
using DiscUtils.Fat;
using DiscUtils.Partitions;
using DiscUtils.Vmdk;

namespace XSharp.Launch.HardDisks
{
    public class VmdkHardDisk : VirtualHardDiskBase
    {
        private Disk mDisk;
        private FatFileSystem mFileSystem;

        public override VirtualDisk VirtualDisk => mDisk;
        public override DiscFileSystem FileSystem => mFileSystem;

        public VmdkHardDisk(string aDiskPath, uint aDiskSize)
            : base(aDiskPath, aDiskSize)
        {
        }

        public override void Initialize(Action<DiscFileSystem> aInitializeHardDiskContents)
        {
            if (IsInitialized)
            {
                throw new Exception("Disk is already initialized!");
            }

            mDisk = Disk.Initialize(DiskPath, DiskSize, DiskCreateType.MonolithicSparse);
            BiosPartitionTable.Initialize(mDisk, WellKnownPartitionType.WindowsFat);

            mFileSystem = FatFileSystem.FormatPartition(mDisk, 0, null);
            aInitializeHardDiskContents(mFileSystem);
        }
    }
}
