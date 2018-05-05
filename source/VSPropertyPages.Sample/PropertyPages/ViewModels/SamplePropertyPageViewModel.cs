using Microsoft.VisualStudio.ProjectSystem;

namespace VSPropertyPages.Sample.PropertyPages
{
    internal class SamplePropertyPageViewModel : PropertyPageViewModel
    {
        public SamplePropertyPageViewModel(
            IPropertyManager propertyManager,
            IProjectThreadingService projectThreadingService)
            : base(propertyManager, projectThreadingService)
        {
        }

        public string TargetFramework
        {
            get => GetProperty("TargetFramework");
            set => SetProperty("TargetFramework", value, nameof(TargetFramework));
        }

        public string AssemblyName
        {
            get => GetProperty("AssemblyName");
            set => SetProperty("AssemblyName", value, nameof(AssemblyName));
        }
    }
}
