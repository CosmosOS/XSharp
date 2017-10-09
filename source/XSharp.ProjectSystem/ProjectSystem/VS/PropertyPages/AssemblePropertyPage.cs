using System;
using System.Runtime.InteropServices;
using Microsoft.VisualStudio.ProjectSystem;

using VSPropertyPages;

namespace XSharp.ProjectSystem.VS.PropertyPages
{
    [Guid(PageGuid)]
    public class AssemblePropertyPage : PropertyPage
    {
        public const string PageGuid = "3e3ca5b3-60f0-4e28-9973-3b77b32f742b";

        public override string PageName => "Assemble";

        public override IPropertyPageUI CreatePropertyPageUI() => new AssemblePropertyPageControl();

        public override PropertyPageViewModel CreatePropertyPageViewModel(
            UnconfiguredProject unconfiguredProject, IProjectThreadingService projectThreadingService) =>
            new AssemblePropertyPageViewModel(new DynamicPropertyManager(unconfiguredProject), projectThreadingService);
    }
}
