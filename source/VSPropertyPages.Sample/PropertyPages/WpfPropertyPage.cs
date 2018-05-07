using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using Microsoft.VisualStudio.ProjectSystem;

namespace VSPropertyPages.Sample.PropertyPages
{
    [Guid(PageGuid)]
    public class WpfPropertyPage : PropertyPageBase
    {
        internal const string PageGuid = "bcddeae3-f743-42a0-8677-bffa37d0f385";

        public override string PageName => "WPF";

        public override IPropertyPageUI CreatePropertyPageUI() =>
            new WpfPropertyPageControl()
            {
                DataContext = new SamplePropertyPageViewModel(PropertyManager, ProjectThreadingService)
            };

        public override IPropertyManager CreatePropertyManager(
            IReadOnlyCollection<ConfiguredProject> configuredProjects) =>
            new DynamicUnconfiguredPropertyManager(configuredProjects.First());
    }
}
