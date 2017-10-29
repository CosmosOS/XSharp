using System;
using System.Runtime.InteropServices;
using Microsoft.VisualStudio.ProjectSystem;

namespace VSPropertyPages.Sample.PropertyPages
{
    [Guid(PageGuid)]
    public class WpfPropertyPage : PropertyPage
    {
        internal const string PageGuid = "bcddeae3-f743-42a0-8677-bffa37d0f385";

        public override string PageName => "WPF";

        public override IPropertyPageUI CreatePropertyPageUI() => new WpfPropertyPageControl();

        public override PropertyPageViewModel CreatePropertyPageViewModel(
            UnconfiguredProject unconfiguredProject, IProjectThreadingService projectThreadingService) =>
            new SamplePropertyPageViewModel(new DynamicPropertyManager(unconfiguredProject), projectThreadingService);
    }
}
