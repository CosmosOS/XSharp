using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Composition;
using System.Threading.Tasks;
using Microsoft.VisualStudio.ProjectSystem;
using Microsoft.VisualStudio.ProjectSystem.Debug;
using Microsoft.VisualStudio.ProjectSystem.Properties;
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

        [ExportPropertyXamlRuleDefinition("XSharp.ProjectSystem, Version=1.0.0.0, Culture=neutral, PublicKeyToken=b94a93fbb8fa3f4f", "XamlRuleToCode:XSharpDebugger.xaml", PropertyPageContexts.Project)]
        [AppliesTo(ProjectCapability.XSharp)]
        private object DebuggerXaml { get { throw new NotImplementedException(); } }

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
