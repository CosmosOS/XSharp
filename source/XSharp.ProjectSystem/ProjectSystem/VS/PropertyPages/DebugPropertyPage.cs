using System;
using System.Runtime.InteropServices;
using Microsoft.VisualStudio.ProjectSystem;

using VSPropertyPages;

namespace XSharp.ProjectSystem.VS.PropertyPages
{
    [Guid(PageGuid)]
    public class DebugPropertyPage : PropertyPage
    {
        public const string PageGuid = "b56385a5-ad14-4f70-bbeb-ab63baee3dc1";

        public override string PageName => "Debug";

        public override IPropertyPageUI CreatePropertyPageUI() => new DebugPropertyPageControl();

        public override PropertyPageViewModel CreatePropertyPageViewModel(
            UnconfiguredProject unconfiguredProject, IProjectThreadingService projectThreadingService) =>
            new DebugPropertyPageViewModel(new DynamicPropertyManager(unconfiguredProject), projectThreadingService);
    }
}
