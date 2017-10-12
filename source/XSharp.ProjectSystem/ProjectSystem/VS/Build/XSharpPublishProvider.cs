using System;
using System.Collections.Immutable;
using System.Composition;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.VisualStudio.ProjectSystem;
using Microsoft.VisualStudio.ProjectSystem.Build;
using Microsoft.VisualStudio.Threading;
using Task = System.Threading.Tasks.Task;

using DiscUtils.Iso9660;

using static XSharp.ProjectSystem.ConfigurationGeneral;

namespace XSharp.ProjectSystem.VS.Build
{
    [Export(typeof(IPublishProvider))]
    [AppliesTo(ProjectCapability.XSharp)]
    internal class XSharpPublishProvider : IPublishProvider
    {
        [Import]
        private ConfiguredProject ConfiguredProject { get; set; }

        [Import]
        private ProjectProperties ProjectProperties { get; set; }
        
        [Import]
        private IProjectThreadingService ProjectThreadingService { get; set; }

        private PublishSettings mPublishSettings;

        public Task<bool> IsPublishSupportedAsync()
        {
            return TplExtensions.TrueTask;
        }

        public async Task PublishAsync(CancellationToken aCancellationToken, TextWriter aOutputPaneWriter)
        {
            var xProjectProperties = await ProjectProperties.GetConfigurationGeneralPropertiesAsync();

            var xOutputISO = await xProjectProperties.OutputISO.GetEvaluatedValueAtEndAsync();
            xOutputISO = ConfiguredProject.UnconfiguredProject.MakeRooted(xOutputISO);

            if (await xProjectProperties.OutputType.GetEvaluatedValueAtEndAsync() == OutputTypeValues.Bootable)
            {
                if (mPublishSettings == null)
                {
                    await aOutputPaneWriter.WriteAsync("Publish settings are null!");
                    return;
                }

                switch (mPublishSettings.PublishType)
                {
                    case PublishType.ISO:
                        await aOutputPaneWriter.WriteLineAsync("Publishing ISO!");

                        if (String.IsNullOrWhiteSpace(mPublishSettings.PublishPath))
                        {
                            throw new Exception($"Invalid publish path! Publish path: '{mPublishSettings.PublishPath}'");
                        }

                        File.Copy(xOutputISO, mPublishSettings.PublishPath, true);

                        break;
                    case PublishType.USB:
                        await aOutputPaneWriter.WriteLineAsync("Publishing USB!");

                        DriveInfo xDriveInfo;

                        try
                        {
                            xDriveInfo = new DriveInfo(mPublishSettings.PublishPath);
                        }
                        catch (ArgumentException)
                        {
                            throw new Exception($"Invalid drive letter! Drive letter: '{mPublishSettings.PublishPath}'");
                        }

                        // todo: format USB drive if requested? how?

                        var xDrivePath = xDriveInfo.RootDirectory.FullName;

                        using (var xStream = File.OpenRead(xOutputISO))
                        {
                            using (var xReader = new CDReader(xStream, true))
                            {
                                foreach (var xFile in xReader.GetFiles(""))
                                {
                                    using (var xFileStream = xReader.OpenFile(xFile, FileMode.Open))
                                    {
                                        using (var xNewFile = File.Create(Path.Combine(xDrivePath, Path.GetFileName(xFile))))
                                        {
                                            await xFileStream.CopyToAsync(xNewFile);
                                        }
                                    }
                                }
                            }
                        }

                        break;
                    case PublishType.PXE:
                        await aOutputPaneWriter.WriteLineAsync("Publishing PXE!");

                        // todo: PXE publish

                        break;
                    default:
                        await aOutputPaneWriter.WriteLineAsync($"Unknown publish type! Publish type: '{mPublishSettings.PublishType}'");
                        break;
                }
            }
            else
            {
                await ConfiguredProject.Services.Build.BuildAsync(ImmutableArray.Create("Publish"), CancellationToken.None, true);
            }

            await aOutputPaneWriter.WriteLineAsync("Publish successful!");
        }

        public async Task<bool> ShowPublishPromptAsync()
        {
            var xProjectProperties = await ProjectProperties.GetConfigurationGeneralPropertiesAsync();

            if (await xProjectProperties.OutputType.GetEvaluatedValueAtEndAsync() == OutputTypeValues.Bootable)
            {
                await ProjectThreadingService.SwitchToUIThread();

                var xBinOutputPath = await xProjectProperties.BinOutputPath.GetEvaluatedValueAtEndAsync();
                var xDefaultIsoPublishPath = ConfiguredProject.UnconfiguredProject.MakeRooted(
                    Path.Combine(xBinOutputPath, Path.ChangeExtension(Path.GetFileName(ConfiguredProject.UnconfiguredProject.FullPath), "iso")));
                var xDefaultPxePublishPath = ConfiguredProject.UnconfiguredProject.MakeRooted(Path.Combine(xBinOutputPath, "PXE Publish"));

                var xDefaultPublishProperties = new DefaultPublishProperties(xDefaultIsoPublishPath, xDefaultPxePublishPath);

                var xPublishWindow = new PublishWindow(xDefaultPublishProperties);
                mPublishSettings = xPublishWindow.ShowModal();

                return mPublishSettings != null;
            }
            else
            {
                return true;
            }
        }
    }
}
