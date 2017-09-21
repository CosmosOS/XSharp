using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.ComponentModel.Composition;
using System.Diagnostics;
using System.Linq;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.VisualStudio.ProjectSystem;
using Microsoft.VisualStudio.ProjectSystem.Build;
using Microsoft.VisualStudio.ProjectSystem.Debug;
using Microsoft.VisualStudio.ProjectSystem.Properties;
using Microsoft.VisualStudio.ProjectSystem.VS.Debug;
using Microsoft.VisualStudio.Threading;

namespace XSharp.ProjectSystem.VS
{
    [ExportDebugger(XSharpDebugger.SchemaName)]
    [AppliesTo(ProjectCapability.XSharp)]
    public class DebugLaunchProvider : DebugLaunchProviderBase
    {
        [ImportingConstructor]
        public DebugLaunchProvider(ConfiguredProject aConfiguredProject)
            : base(aConfiguredProject)
        {
        }

        //[ExportPropertyXamlRuleDefinition("XSharp.ProjectSystem, Version=1.0.0.0, Culture=neutral, PublicKeyToken=b94a93fbb8fa3f4f", "XamlRuleToCode:XSharpDebugger.xaml", PropertyPageContexts.Project)]
        //[AppliesTo(ProjectCapability.XSharp)]
        //private object DebuggerXaml { get { throw new NotImplementedException(); } }

        [Import]
        private ProjectProperties ProjectProperties { get; set; }

        public override Task<bool> CanLaunchAsync(DebugLaunchOptions aLaunchOptions)
        {
            return TplExtensions.TrueTask;
        }

        public override async Task<IReadOnlyList<IDebugLaunchSettings>> QueryDebugTargetsAsync(DebugLaunchOptions aLaunchOptions)
        {
            var xConfiguration = await ProjectProperties.GetConfigurationGeneralPropertiesAsync();
            var xOutputType = await xConfiguration.OutputType.GetEvaluatedValueAtEndAsync();

            if (xOutputType != "Application" && xOutputType != "Bootable")
            {
                throw new Exception($"Project cannot be launched! Output type: '{xOutputType}'.");
            }

            await ConfiguredProject.Services.Build.BuildAsync(ImmutableArray.Create("Run"), CancellationToken.None, true);

            if (!aLaunchOptions.HasFlag(DebugLaunchOptions.NoDebug))
            {
                var xBinaryOutput = await xConfiguration.BinaryOutput.GetEvaluatedValueAtEndAsync();

                var xDebugSettings = new DebugLaunchSettings(aLaunchOptions);
                xDebugSettings.LaunchOperation = DebugLaunchOperation.AlreadyRunning;
                xDebugSettings.CurrentDirectory = Path.GetFullPath(Path.GetDirectoryName(xBinaryOutput));

                if (xOutputType == "Bootable")
                {
                    // todo: implement
                    //xDebugSettings.LaunchDebugEngineGuid = XSharpDebuggerGuid;

                    return new DebugLaunchSettings[0];
                }
                else
                {
                    var xProcessId = Process.GetProcessesByName(Path.GetFileName(xBinaryOutput)).OrderBy(p => p.StartTime).Last().Id;

                    xDebugSettings.ProcessId = xProcessId;
                    xDebugSettings.LaunchDebugEngineGuid = DebuggerEngines.NativeOnlyEngine;
                }

                return ImmutableArray.Create(xDebugSettings);
            }

            return new DebugLaunchSettings[0];
        }
    }
}
