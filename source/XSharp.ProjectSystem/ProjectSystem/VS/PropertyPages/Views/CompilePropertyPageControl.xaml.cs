using System;
using System.Threading.Tasks;

using VSPropertyPages;

namespace XSharp.ProjectSystem.VS.PropertyPages
{
    /// <summary>
    /// Interaction logic for CompilePropertyPageControl.xaml
    /// </summary>
    internal partial class CompilePropertyPageControl : WpfPropertyPageUI
    {
        public CompilePropertyPageControl()
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
