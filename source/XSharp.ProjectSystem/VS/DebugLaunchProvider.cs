using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Composition;
using System.Threading.Tasks;
using Microsoft.VisualStudio.ProjectSystem;
using Microsoft.VisualStudio.ProjectSystem.Debug;
using Microsoft.VisualStudio.ProjectSystem.VS.Debug;
using Microsoft.VisualStudio.Threading;

namespace XSharp.ProjectSystem.VS
{
    [ExportDebugger(XSharpDebugger.SchemaName)]
    [AppliesTo(ProjectCapability.XSharp)]
    internal class DebugLaunchProvider : DebugLaunchProviderBase
    {
        [Import]
        private ProjectProperties ProjectProperties { get; set; }

        [ImportingConstructor]
        public DebugLaunchProvider(ConfiguredProject configuredProject)
            : base(configuredProject)
        {
        }

        public override Task<bool> CanLaunchAsync(DebugLaunchOptions aLaunchOptions)
        {
            return TplExtensions.TrueTask;
        }

        public override async Task<IReadOnlyList<IDebugLaunchSettings>> QueryDebugTargetsAsync(DebugLaunchOptions aLaunchOptions)
        {
            var xDebugSettings = new DebugLaunchSettings(aLaunchOptions);

            var xDebuggerProperties = await ProjectProperties.GetXSharpDebuggerPropertiesAsync();
            xDebugSettings.CurrentDirectory = await xDebuggerProperties.RunWorkingDirectory.GetEvaluatedValueAtEndAsync();

            if (aLaunchOptions.HasFlag(DebugLaunchOptions.NoDebug))
            {

            }
            else
            {

            }

            xDebugSettings.Executable = "cmd";

            xDebugSettings.LaunchOperation = DebugLaunchOperation.CreateProcess;
            xDebugSettings.LaunchDebugEngineGuid = DebuggerEngines.NativeOnlyEngine;

            return ImmutableArray.Create(xDebugSettings);
        }
    }
}
