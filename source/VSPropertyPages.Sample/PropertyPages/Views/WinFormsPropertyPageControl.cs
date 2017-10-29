using System;
using System.Threading.Tasks;

namespace VSPropertyPages.Sample.PropertyPages
{
    internal partial class WinFormsPropertyPageControl : WinFormsPropertyPageUI
    {
        public WinFormsPropertyPageControl()
        {
            InitializeComponent();
        }

        public override Task SetViewModelAsync(PropertyPageViewModel propertyPageViewModel)
        {
            textBox1.DataBindings.Add("Text", propertyPageViewModel, nameof(SamplePropertyPageViewModel.TargetFramework));
            textBox2.DataBindings.Add("Text", propertyPageViewModel, nameof(SamplePropertyPageViewModel.AssemblyName));

            return Task.CompletedTask;
        }
    }
}
