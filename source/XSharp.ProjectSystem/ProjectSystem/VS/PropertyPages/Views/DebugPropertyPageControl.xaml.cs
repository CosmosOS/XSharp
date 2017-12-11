using System;
using System.Threading.Tasks;

using VSPropertyPages;

namespace XSharp.ProjectSystem.VS.PropertyPages
{
    /// <summary>
    /// Interaction logic for DebugPropertyPageControl.xaml
    /// </summary>
    internal partial class DebugPropertyPageControl : WpfPropertyPageUI
    {
        public DebugPropertyPageControl()
        {
            InitializeComponent();
        }

        public override Task SetViewModelAsync(PropertyPageViewModel propertyPageViewModel)
        {
            DataContext = propertyPageViewModel;
            return Task.CompletedTask;
        }
    }
}
