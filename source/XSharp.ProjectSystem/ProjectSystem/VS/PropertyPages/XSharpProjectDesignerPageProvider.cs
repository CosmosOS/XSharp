using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Composition;
using System.Threading.Tasks;
using Microsoft.VisualStudio.ProjectSystem;
using Microsoft.VisualStudio.ProjectSystem.VS.Properties;

using VSPropertyPages;

namespace XSharp.ProjectSystem.VS.PropertyPages
{
    [Export(typeof(IVsProjectDesignerPageProvider))]
    [AppliesTo(ProjectCapability.XSharpAppDesigner)]
    internal class XSharpProjectDesignerPageProvider : IVsProjectDesignerPageProvider
    {
        private readonly IPageMetadata CompilePage = new PropertyPageMetadata("Compile", CompilePropertyPage.PageGuid, 0, false);
        private readonly IPageMetadata AssemblePage = new PropertyPageMetadata("Assemble", AssemblePropertyPage.PageGuid, 1, false);

        public Task<IReadOnlyCollection<IPageMetadata>> GetPagesAsync()
        {
            var xPages = ImmutableArray.Create(CompilePage, AssemblePage);
            return Task.FromResult<IReadOnlyCollection<IPageMetadata>>(xPages);
        }
    }
}
