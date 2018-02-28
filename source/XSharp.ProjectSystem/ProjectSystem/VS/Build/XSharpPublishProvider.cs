using System;
using System.Collections.Immutable;
using System.ComponentModel.Composition;
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
    [AppliesTo(ProjectCapability.XSharpAndRunningInVisualStudio)]
    internal class XSharpPublishProvider : IPublishProvider
    {
        [Import]
        private ConfiguredProject ConfiguredProject { get; set; }

        [Import]
        private ProjectProperties ProjectProperties { get; set; }

        [Import]
        private IProjectThreadingService ProjectThreadingService { get; set; }

        private PublishSettings mPublishSettings;

        public Task<bool> IsPublishSupportedAsync() => TplExtensions.TrueTask;

        public async Task PublishAsync(CancellationToken aCancellationToken, TextWriter aOutputPaneWriter)
        {
            var xProjectProperties = await ProjectProperties.GetConfigurationGeneralPropertiesAsync().ConfigureAwait(false);
            var xOutputType = await xProjectProperties.OutputType.GetEvaluatedValueAtEndAsync().ConfigureAwait(false);

            var xOutputISO = await xProjectProperties.OutputISO.GetEvaluatedValueAtEndAsync().ConfigureAwait(false);
            xOutputISO = ConfiguredProject.UnconfiguredProject.MakeRooted(xOutputISO);

            if (String.Equals(xOutputType, OutputTypeValues.Bootable, StringComparison.OrdinalIgnoreCase))
            {
                if (mPublishSettings == null)
                {
                    await aOutputPaneWriter.WriteAsync("Publish settings are null!").ConfigureAwait(false);
                    return;
                }

                switch (mPublishSettings.PublishType)
                {
                    case PublishType.ISO:
                        await aOutputPaneWriter.WriteLineAsync("Publishing ISO!").ConfigureAwait(false);

                        if (String.IsNullOrWhiteSpace(mPublishSettings.PublishPath))
                        {
                            throw new Exception($"Invalid publish path! Publish path: '{mPublishSettings.PublishPath}'");
                        }

                        File.Copy(xOutputISO, mPublishSettings.PublishPath, true);

                        break;
                    case PublishType.USB:
                        await aOutputPaneWriter.WriteLineAsync("Publishing USB!").ConfigureAwait(false);

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
                                            await xFileStream.CopyToAsync(xNewFile).ConfigureAwait(false);
                                        }
                                    }
                                }
                            }
                        }

                        break;
                    default:
                        await aOutputPaneWriter.WriteLineAsync(
                            $"Unknown publish type! Publish type: '{mPublishSettings.PublishType}'").ConfigureAwait(false);
                        break;
                }
            }
            else
            {
                await ConfiguredProject.Services.Build.BuildAsync(
                    ImmutableArray.Create("Publish"), CancellationToken.None, true).ConfigureAwait(false);
            }

            await aOutputPaneWriter.WriteLineAsync("Publish successful!").ConfigureAwait(false);
        }

        public async Task<bool> ShowPublishPromptAsync()
        {
            var xProjectProperties = await ProjectProperties.GetConfigurationGeneralPropertiesAsync().ConfigureAwait(false);
            var xOutputType = await xProjectProperties.OutputType.GetEvaluatedValueAtEndAsync().ConfigureAwait(false);

            if (String.Equals(xOutputType, OutputTypeValues.Bootable, StringComparison.OrdinalIgnoreCase))
            {
                await ProjectThreadingService.SwitchToUIThread();

                var xBinOutputPath = await xProjectProperties.BinOutputPath.GetEvaluatedValueAtEndAsync().ConfigureAwait(false);
                var xDefaultIsoPublishPath = ConfiguredProject.UnconfiguredProject.MakeRooted(
                    Path.Combine(xBinOutputPath, Path.ChangeExtension(Path.GetFileName(ConfiguredProject.UnconfiguredProject.FullPath), "iso")));

                var xDefaultPublishProperties = new DefaultPublishProperties(xDefaultIsoPublishPath);

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
