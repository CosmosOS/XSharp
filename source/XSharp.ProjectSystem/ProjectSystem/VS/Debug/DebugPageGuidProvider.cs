using System;
using System.ComponentModel.Composition;
using System.Threading.Tasks;
using Microsoft.VisualStudio.ProjectSystem;
using Microsoft.VisualStudio.ProjectSystem.VS.Debug;

using XSharp.ProjectSystem.VS.PropertyPages;

namespace XSharp.ProjectSystem.VS.Debug
{
    [Export(typeof(IDebugPageGuidProvider))]
    [AppliesTo(ProjectCapability.XSharpAndRunningInVisualStudio)]
    internal class DebugPageGuidProvider : IDebugPageGuidProvider
    {
        private static readonly Task<Guid> GuidTask = Task.FromResult(new Guid(DebugPropertyPage.PageGuid));

        public Task<Guid> GetDebugPropertyPageGuidAsync() => GuidTask;
    }
}
