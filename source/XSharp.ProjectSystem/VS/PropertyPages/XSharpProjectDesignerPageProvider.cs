using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Composition;
using System.Threading.Tasks;
using Microsoft.VisualStudio.ProjectSystem;
using Microsoft.VisualStudio.ProjectSystem.VS.Properties;

namespace XSharp.ProjectSystem.VS.PropertyPages
{
    [Export(typeof(IVsProjectDesignerPageProvider))]
    [AppliesTo(ProjectCapability.XSharpAppDesigner)]
    internal class XSharpProjectDesignerPageProvider : IVsProjectDesignerPageProvider
    {
        private readonly IPageMetadata CompilePage = new PropertyPageMetadata("Compile", new Guid(CompilePropertyPage.PageGuid), 0, false);

        public Task<IReadOnlyCollection<IPageMetadata>> GetPagesAsync()
        {
            var xPages = ImmutableArray.Create(CompilePage);
            return Task.FromResult<IReadOnlyCollection<IPageMetadata>>(xPages);
        }
    }
}
