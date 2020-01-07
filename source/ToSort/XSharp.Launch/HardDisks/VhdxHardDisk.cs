using System;
using System.IO;

using DiscUtils;
using DiscUtils.Partitions;
using DiscUtils.Vhdx;
using DiscUtils.Fat;
using DiscUtils.Streams;

namespace XSharp.Launch.HardDisks
{
    public class VhdxHardDisk : VirtualHardDiskBase
    {
        private Disk mDisk;
        private FatFileSystem mFileSystem;

        public override VirtualDisk VirtualDisk => mDisk;
        public override DiscFileSystem FileSystem => mFileSystem;

        public VhdxHardDisk(string aDiskPath, uint aDiskSize)
            : base(aDiskPath, aDiskSize)
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
