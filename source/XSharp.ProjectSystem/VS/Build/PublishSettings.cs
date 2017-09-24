using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XSharp.ProjectSystem.VS.Build
{
    public enum PublishType
    {
        USB,
        PXE
    }

    public class PublishSettings
    {
        public PublishType PublishType { get; }

        public PublishSettings(PublishType aPublishType)
        {
            PublishType = aPublishType;
        }
    }
}
