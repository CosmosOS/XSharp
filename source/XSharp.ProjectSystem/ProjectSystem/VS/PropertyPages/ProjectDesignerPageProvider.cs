using System.Collections.Generic;
using System.Collections.Immutable;
using System.ComponentModel.Composition;
using System.Threading.Tasks;
using Microsoft.VisualStudio.ProjectSystem;
using Microsoft.VisualStudio.ProjectSystem.VS.Properties;

using VSPropertyPages;

namespace XSharp.ProjectSystem.VS.PropertyPages
{
    [Export(typeof(IVsProjectDesignerPageProvider))]
    [AppliesTo(ProjectCapability.XSharpAndAppDesigner)]
    internal class ProjectDesignerPageProvider : IVsProjectDesignerPageProvider
    {
        private static readonly IPageMetadata CompilePage = new PropertyPageMetadata(
            "Compile", CompilePropertyPage.PageGuid, 0, true);
        private static readonly IPageMetadata AssemblePage = new PropertyPageMetadata(
            "Assemble", AssemblePropertyPage.PageGuid, 1, true);
        private static readonly IPageMetadata DebugPage = new PropertyPageMetadata(
            "Debug", DebugPropertyPage.PageGuid, 2, false);

        public Task<IReadOnlyCollection<IPageMetadata>> GetPagesAsync()
        {
            return Task.FromResult<IReadOnlyCollection<IPageMetadata>>(
                ImmutableArray.Create(CompilePage, AssemblePage, DebugPage));
        }
    }
}
