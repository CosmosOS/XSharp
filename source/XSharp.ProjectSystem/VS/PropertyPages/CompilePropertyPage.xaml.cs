using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.InteropServices;
using Microsoft.VisualStudio.ProjectSystem;

namespace XSharp.ProjectSystem.VS.PropertyPages
{
    /// <summary>
    /// Interaction logic for CompilePropertyPage.xaml
    /// </summary>
    [Guid(PageGuid)]
    internal partial class CompilePropertyPage : PropertyPage
    {
        public const string PageGuid = "d1d2c48a-0870-4d32-b9c8-d3775d0fc5bf";

        protected override string PageName => "Compile";

        private CompilePropertyPageViewModel mViewModel;
        protected override PropertyPageViewModel ViewModel => mViewModel;

        public CompilePropertyPage()
        {
            InitializeComponent();
        }

        protected override void SetObjects(UnconfiguredProject aUnconfiguredProject)
        {
            mViewModel = new CompilePropertyPageViewModel(aUnconfiguredProject);
        }

        private class CompilePropertyPageViewModel : PropertyPageViewModel
        {
            public CompilePropertyPageViewModel(UnconfiguredProject aUnconfiguredProject)
                : base(aUnconfiguredProject)
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
}
