using System;
using System.Threading.Tasks;

using VSPropertyPages;

namespace XSharp.ProjectSystem.VS.PropertyPages
{
    /// <summary>
    /// Interaction logic for AssemblePropertyPageControl.xaml
    /// </summary>
    internal partial class AssemblePropertyPageControl : WpfPropertyPageUI
    {
        public AssemblePropertyPageControl()
        {
            InitializeComponent();
        }

        public override Task SetViewModelAsync(PropertyPageViewModel aPropertyPageViewModel)
        {
            DataContext = aPropertyPageViewModel;
            return Task.CompletedTask;
        }
    }
}
