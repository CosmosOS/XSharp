using Microsoft.VisualStudio.ProjectSystem;

using VSPropertyPages;

namespace XSharp.ProjectSystem.VS.PropertyPages
{
    internal class DebugPropertyPageViewModel : PropertyPageViewModel
    {
        public DebugPropertyPageViewModel(IPropertyManager aPropertyManager, IProjectThreadingService aProjectThreadingService)
            : base(aPropertyManager, aProjectThreadingService)
        {
        }
    }
}
