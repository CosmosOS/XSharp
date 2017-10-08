using System;
using System.Runtime.InteropServices;
using Microsoft.VisualStudio.ProjectSystem;

namespace VSPropertyPages.Sample.PropertyPages
{
    [Guid(PageGuid)]
    public class WinFormsPropertyPage : PropertyPage
    {
        internal const string PageGuid = "296babc3-11c1-4115-b350-003a6536a0f7";

        public override string PageName => "WinForms";

        public override IPropertyPageUI CreatePropertyPageUI() => new WinFormsPropertyPageControl();

        public override PropertyPageViewModel CreatePropertyPageViewModel(
            UnconfiguredProject unconfiguredProject, IProjectThreadingService projectThreadingService) =>
            new SamplePropertyPageViewModel(new DynamicPropertyManager(unconfiguredProject), projectThreadingService);
    }
}
