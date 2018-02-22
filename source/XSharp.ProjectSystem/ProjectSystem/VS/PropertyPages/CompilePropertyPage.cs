using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.VisualStudio.ProjectSystem;

using VSPropertyPages;

namespace XSharp.ProjectSystem.VS.PropertyPages
{
    [Guid(PageGuid)]
    public class CompilePropertyPage : PropertyPageBase
    {
        public const string PageGuid = "d1d2c48a-0870-4d32-b9c8-d3775d0fc5bf";

        public override string PageName => "Compile";

        public override IPropertyPageUI CreatePropertyPageUI() =>
            new CompilePropertyPageControl()
            {
                DataContext = new CompilePropertyPageViewModel(PropertyManager, ProjectThreadingService)
            };

        public override IPropertyManager CreatePropertyManager(
            IReadOnlyCollection<ConfiguredProject> configuredProjects) =>
            new DynamicConfiguredPropertyManager(UnconfiguredProject, configuredProjects);
    }
}
