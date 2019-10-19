using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.VisualStudio.ProjectSystem;

using VSPropertyPages;

namespace XSharp.ProjectSystem.VS.PropertyPages
{
    [Guid(PageGuid)]
    public class AssemblePropertyPage : PropertyPageBase
    {
        public const string PageGuid = "3e3ca5b3-60f0-4e28-9973-3b77b32f742b";

        public override string PageName => "Assemble";

        public override IPropertyPageUI CreatePropertyPageUI() =>
            new AssemblePropertyPageControl()
            {
                DataContext = new AssemblePropertyPageViewModel(PropertyManager, ProjectThreadingService)
            };

        public override IPropertyManager CreatePropertyManager(
            IReadOnlyCollection<ConfiguredProject> configuredProjects) =>
            new DynamicConfiguredPropertyManager(UnconfiguredProject, configuredProjects);
    }
}
