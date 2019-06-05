using System;

using DiscUtils;

namespace XSharp.Launch.HardDisks
{
    public abstract class VirtualHardDiskBase : IDisposable
    {
        public string DiskPath { get; private set; }
        public uint DiskSize { get; private set; }
        public bool IsInitialized => VirtualDisk != null;

        public abstract VirtualDisk VirtualDisk { get; }
        public abstract DiscFileSystem FileSystem { get; }

        protected VirtualHardDiskBase(string aDiskPath, uint aDiskSize)
        {
            DiskPath = aDiskPath;
            DiskSize = aDiskSize;
        }

        public abstract void Initialize(Action<DiscFileSystem> aInitializeHardDiskContents);
        
        public void InitializeWithSampleContent()
        {
            Initialize(CreateSampleContent);
        }

        protected virtual void Dispose(bool aDisposing)
        {
            if (aDisposing)
            {
                VirtualDisk?.Dispose();
                FileSystem?.Dispose();
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private static void CreateSampleContent(DiscFileSystem aFileSystem)
        {
            /*
                \Dir Testing\
                \test\
                \test\DirInTest\
                \test\DirInTest\Readme.txt -> contains 40 bytes of text
                \Kudzu.txt -> contains 12 bytes of text
                \Root.txt -> contains 5 bytes of text
            */

            // LFN not yet supported: https://github.com/DiscUtils/DiscUtils/pull/71
            //aFileSystem.CreateDirectory(@"DirTesting");
            aFileSystem.CreateDirectory(@"test");
            //aFileSystem.CreateDirectory(@"test\DirInTest");

            //using (var xWriter = aFileSystem.GetFileInfo(@"test\DirInTest\Readme.txt").CreateText())
            //{
            //    xWriter.Write(@"This file is located in \test\DirInTest!");
            //}

            using (var xWriter = aFileSystem.GetFileInfo(@"Kudzu.txt").CreateText())
            {
                xWriter.Write("Hello Cosmos");
            }

            using (var xWriter = aFileSystem.GetFileInfo(@"Root.txt").CreateText())
            {
                xWriter.Write("Hello");
            }
        }
    }
}
