using System;
using System.Composition;
using System.Threading.Tasks;
using Microsoft.VisualStudio.ProjectSystem;
using Microsoft.VisualStudio.ProjectSystem.VS.Debug;

using XSharp.ProjectSystem.VS.PropertyPages;

namespace XSharp.ProjectSystem.VS.Debug
{
    [Export(typeof(IDebugPageGuidProvider))]
    [AppliesTo(ProjectCapability.XSharpAndRunningInVisualStudio)]
    internal class XSharpDebugPageGuidProvider : IDebugPageGuidProvider
    {
        private static readonly Task<Guid> Guid = Task.FromResult(new Guid(DebugPropertyPage.PageGuid));

        public Task<Guid> GetDebugPropertyPageGuidAsync() => Guid;
    }
}
