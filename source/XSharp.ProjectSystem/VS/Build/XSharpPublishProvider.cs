using System;
using System.Collections.Immutable;
using System.Composition;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.VisualStudio.ProjectSystem;
using Microsoft.VisualStudio.ProjectSystem.Build;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Threading;
using Task = System.Threading.Tasks.Task;

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
        private SVsServiceProvider ServiceProvider { get; set; }

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

            if (await xProjectProperties.OutputType.GetEvaluatedValueAtEndAsync() == "Bootable")
            {
                if (mPublishSettings == null)
                {
                    await aOutputPaneWriter.WriteAsync("Publish settings are null!");
                    return;
                }

                switch (mPublishSettings.PublishType)
                {
                    case PublishType.USB:
                        await aOutputPaneWriter.WriteLineAsync("Publishing USB!");
                        break;
                    case PublishType.PXE:
                        await aOutputPaneWriter.WriteLineAsync("Publishing PXE!");
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

            if (await xProjectProperties.OutputType.GetEvaluatedValueAtEndAsync() == "Bootable")
            {
                await ProjectThreadingService.SwitchToUIThread();

                var xPublishWindow = new PublishWindow();
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
