using Microsoft.VisualStudio.ProjectSystem;

using VSPropertyPages;

namespace XSharp.ProjectSystem.VS.PropertyPages
{
    internal class CompilePropertyPageViewModel : PropertyPageViewModel
    {
        public CompilePropertyPageViewModel(IPropertyManager aPropertyManager, IProjectThreadingService aProjectThreadingService)
            : base(aPropertyManager, aProjectThreadingService)
        {
        }

        public string OutputType
        {
            get => GetProperty("OutputType");
            set => SetProperty("OutputType", value);
        }

        public string OutputName
        {
            get => GetProperty("OutputName");
            set => SetProperty("OutputName", value);
        }
    }
}
