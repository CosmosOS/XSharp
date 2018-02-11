using System.Collections.Generic;
using System.Collections.Immutable;
using System.Composition;
using System.Threading.Tasks;
using Microsoft.VisualStudio.ProjectSystem;
using Microsoft.VisualStudio.ProjectSystem.VS.Properties;

namespace VSPropertyPages.Sample.PropertyPages
{
    [Export(typeof(IVsProjectDesignerPageProvider))]
    [AppliesTo(ProjectCapability.VSPropertyPagesSampleAndAppDesigner)]
    internal class VSPropertyPagesSampleProjectDesignerPageProvider : IVsProjectDesignerPageProvider
    {
        private readonly IPageMetadata WpfPage = new PropertyPageMetadata("WPF", WpfPropertyPage.PageGuid, int.MinValue, false);
        private readonly IPageMetadata WinFormsPage = new PropertyPageMetadata("WinForms", WinFormsPropertyPage.PageGuid, int.MinValue + 1, false);

        public Task<IReadOnlyCollection<IPageMetadata>> GetPagesAsync()
        {
            var propertyPages = ImmutableArray.Create(WpfPage, WinFormsPage);
            return Task.FromResult<IReadOnlyCollection<IPageMetadata>>(propertyPages);
        }
    }
}
