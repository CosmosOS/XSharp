using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.VisualStudio.ProjectSystem;

using VSPropertyPages;

namespace XSharp.ProjectSystem.VS.PropertyPages
{
    [Guid(PageGuid)]
    public class DebugPropertyPage : PropertyPageBase
    {
        public const string PageGuid = "b56385a5-ad14-4f70-bbeb-ab63baee3dc1";

        public override string PageName => "Debug";

        public override IPropertyPageUI CreatePropertyPageUI() =>
            new DebugPropertyPageControl()
            {
                DataContext = new DebugPropertyPageViewModel(PropertyManager, ProjectThreadingService)
            };

        public override IPropertyManager CreatePropertyManager(
            IReadOnlyCollection<ConfiguredProject> configuredProjects) =>
            new DynamicConfiguredPropertyManager(UnconfiguredProject, configuredProjects);
    }
}
