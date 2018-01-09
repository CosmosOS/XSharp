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
    internal class XSharpDebugLaunchProvider : DebugLaunchProviderBase
    {
        private ProjectProperties mProjectProperties;

        [ImportingConstructor]
        public XSharpDebugLaunchProvider(ConfiguredProject aConfiguredProject, ProjectProperties aProjectProperties)
            : base(aConfiguredProject)
        {
            mProjectProperties = aProjectProperties;
        }

        public override Task<bool> CanLaunchAsync(DebugLaunchOptions aLaunchOptions) => TplExtensions.TrueTask;

        public override async Task<IReadOnlyList<IDebugLaunchSettings>> QueryDebugTargetsAsync(DebugLaunchOptions aLaunchOptions)
        {
            var xProjectProperties = await mProjectProperties.GetConfigurationGeneralPropertiesAsync();
            var xOutputType = await xProjectProperties.OutputType.GetEvaluatedValueAtEndAsync();

            if (xOutputType != OutputTypeValues.Application && xOutputType != OutputTypeValues.Bootable)
            {
                throw new Exception($"Project cannot be launched! Output type: '{xOutputType}'.");
            }

            if (xOutputType == OutputTypeValues.Bootable)
            {
                // todo: using debugger for this would be better
                await ConfiguredProject.Services.Build.BuildAsync(ImmutableArray.Create("Run"), CancellationToken.None, true);
            }

            if (!aLaunchOptions.HasFlag(DebugLaunchOptions.NoDebug))
            {
                var xBinaryOutput = await xProjectProperties.BinaryOutput.GetEvaluatedValueAtEndAsync();
                xBinaryOutput = Path.GetFullPath(xBinaryOutput);

                var xDebugSettings = new DebugLaunchSettings(aLaunchOptions);
                xDebugSettings.LaunchOperation = DebugLaunchOperation.AlreadyRunning;
                xDebugSettings.CurrentDirectory = Path.GetDirectoryName(xBinaryOutput);

                if (xOutputType == OutputTypeValues.Bootable)
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
