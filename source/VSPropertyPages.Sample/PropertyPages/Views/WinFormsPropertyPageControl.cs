namespace VSPropertyPages.Sample.PropertyPages
{
    internal partial class WinFormsPropertyPageControl : WinFormsPropertyPageUI
    {
        public WinFormsPropertyPageControl(SamplePropertyPageViewModel viewModel)
        {
            InitializeComponent();
            InitializeBindings(viewModel);
        }

        private void InitializeBindings(SamplePropertyPageViewModel viewModel)
        {
            textBox1.DataBindings.Add("Text", viewModel, nameof(SamplePropertyPageViewModel.TargetFramework));
            textBox2.DataBindings.Add("Text", viewModel, nameof(SamplePropertyPageViewModel.AssemblyName));
        }
    }
}
