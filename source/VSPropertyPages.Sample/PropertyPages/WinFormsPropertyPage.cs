using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using Microsoft.VisualStudio.ProjectSystem;

namespace VSPropertyPages.Sample.PropertyPages
{
    [Guid(PageGuid)]
    public class WinFormsPropertyPage : PropertyPageBase
    {
        internal const string PageGuid = "296babc3-11c1-4115-b350-003a6536a0f7";

        public override string PageName => "WinForms";

        public override IPropertyPageUI CreatePropertyPageUI() =>
            new WinFormsPropertyPageControl(
                new SamplePropertyPageViewModel(PropertyManager, ProjectThreadingService));

        public override IPropertyManager CreatePropertyManager(
            IReadOnlyCollection<ConfiguredProject> configuredProjects) =>
            new DynamicUnconfiguredPropertyManager(configuredProjects.First());
    }
}
