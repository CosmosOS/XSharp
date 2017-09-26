using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XSharp.ProjectSystem.VS.Build
{
    public enum PublishType
    {
        ISO,
        USB,
        PXE
    }

    public class PublishSettings
    {
        public PublishType PublishType { get; }
        public string PublishPath { get; }
        public bool FormatUsbDrive { get; }

        public PublishSettings(PublishType aPublishType, string aPublishPath, bool aFormatUsbDrive)
        {
            PublishType = aPublishType;
            PublishPath = aPublishPath;
            FormatUsbDrive = aFormatUsbDrive;
        }
    }
}
