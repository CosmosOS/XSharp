namespace XSharp.ProjectSystem.VS.Build
{
    public enum PublishType
    {
        ISO,
        USB,
        PXE
    }

    internal class PublishSettings
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
