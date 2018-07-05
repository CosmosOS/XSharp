using Microsoft.VisualStudio.ProjectSystem;

namespace VSPropertyPages.Sample.PropertyPages
{
    internal class SamplePropertyPageViewModel : PropertyPageViewModel
    {
        private const string TargetFrameworkProperty = "TargetFramework";
        private const string AssemblyNameProperty = "AssemblyName";

        public SamplePropertyPageViewModel(
            IPropertyManager propertyManager,
            IProjectThreadingService projectThreadingService)
            : base(propertyManager, projectThreadingService)
        {
        }

        public string TargetFramework
        {
            get => GetProperty(TargetFrameworkProperty);
            set => SetProperty(TargetFrameworkProperty, value, nameof(TargetFramework));
        }

        public string AssemblyName
        {
            get => GetProperty(AssemblyNameProperty);
            set => SetProperty(AssemblyNameProperty, value, nameof(AssemblyName));
        }
    }
}
