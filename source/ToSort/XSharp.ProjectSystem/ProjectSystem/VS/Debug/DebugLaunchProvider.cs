using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.ComponentModel.Composition;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.VisualStudio.ProjectSystem;
using Microsoft.VisualStudio.ProjectSystem.Debug;
using Microsoft.VisualStudio.ProjectSystem.VS.Debug;
using Microsoft.VisualStudio.Threading;

using static XSharp.ProjectSystem.ConfigurationGeneral;

namespace XSharp.ProjectSystem.VS.Debug
{
    [ExportDebugger(XSharpDebugger.SchemaName)]
    [AppliesTo(ProjectCapability.XSharp)]
    internal class DebugLaunchProvider : DebugLaunchProviderBase
    {
        private ProjectProperties mProjectProperties;

        [ImportingConstructor]
        public DebugLaunchProvider(ConfiguredProject aConfiguredProject, ProjectProperties aProjectProperties)
            : base(aConfiguredProject)
        {
            mProjectProperties = aProjectProperties;
        }

        public override Task<bool> CanLaunchAsync(DebugLaunchOptions aLaunchOptions) => TplExtensions.TrueTask;

        public override async Task<IReadOnlyList<IDebugLaunchSettings>> QueryDebugTargetsAsync(DebugLaunchOptions aLaunchOptions)
        {
            var xProjectProperties = await mProjectProperties.GetConfigurationGeneralPropertiesAsync().ConfigureAwait(false);
            var xOutputType = await xProjectProperties.OutputType.GetEvaluatedValueAtEndAsync().ConfigureAwait(false);
            var xIsBootable = String.Equals(xOutputType, OutputTypeValues.Bootable, StringComparison.OrdinalIgnoreCase);

            if (!String.Equals(xOutputType, OutputTypeValues.Application, StringComparison.OrdinalIgnoreCase) && !xIsBootable)
            {
                throw new Exception($"Project cannot be launched! Output type: '{xOutputType}'.");
            }

            if (xIsBootable)
            {
                // todo: using debugger for this would be better
                await ConfiguredProject.Services.Build.BuildAsync(
                    ImmutableArray.Create("Run"), CancellationToken.None, true).ConfigureAwait(false);
            }

            if (!aLaunchOptions.HasFlag(DebugLaunchOptions.NoDebug))
            {
                var xBinaryOutput = await xProjectProperties.BinaryOutput.GetEvaluatedValueAtEndAsync().ConfigureAwait(false);
                xBinaryOutput = Path.GetFullPath(xBinaryOutput);

                var xDebugSettings = new DebugLaunchSettings(aLaunchOptions)
                {
                    LaunchOperation = DebugLaunchOperation.AlreadyRunning,
                    CurrentDirectory = Path.GetDirectoryName(xBinaryOutput)
                };

                if (xIsBootable)
                {
                    // todo: implement
                    //xDebugSettings.LaunchDebugEngineGuid = XSharpDebuggerGuid;

                    return Array.Empty<DebugLaunchSettings>();
                }
                else
                {
                    xDebugSettings.Executable = xBinaryOutput;
                }

                return ImmutableArray.Create(xDebugSettings);
            }

            return Array.Empty<DebugLaunchSettings>();
        }
    }
}
