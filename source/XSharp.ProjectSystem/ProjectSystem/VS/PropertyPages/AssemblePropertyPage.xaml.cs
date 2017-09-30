using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Runtime.InteropServices;
using System.Windows.Input;
using Microsoft.VisualStudio.ProjectSystem;
using Microsoft.Win32;

using static XSharp.ProjectSystem.ConfigurationGeneral;

namespace XSharp.ProjectSystem.VS.PropertyPages
{
    /// <summary>
    /// Interaction logic for CompilePropertyPage.xaml
    /// </summary>
    [Guid(PageGuid)]
    internal partial class AssemblePropertyPage : PropertyPage
    {
        public const string PageGuid = "3e3ca5b3-60f0-4e28-9973-3b77b32f742b";

        protected override string PageName => "Assemble";

        private AssemblePropertyPageViewModel mViewModel;
        protected override PropertyPageViewModel ViewModel => mViewModel;

        public AssemblePropertyPage()
        {
            InitializeComponent();
        }

        protected override void SetObjects(UnconfiguredProject aUnconfiguredProject)
        {
            mViewModel = new AssemblePropertyPageViewModel(aUnconfiguredProject);
        }
    }

    internal class AssemblePropertyPageViewModel : PropertyPageViewModel
    {
        public AssemblePropertyPageViewModel(UnconfiguredProject aUnconfiguredProject)
            : base(aUnconfiguredProject)
        {
        }

        public bool Assemble
        {
            get => Boolean.Parse(GetProperty("Assemble"));
            set => SetProperty("Assemble", value.ToString());
        }

        public string Assembler
        {
            get => GetProperty("Assembler");
            set => SetProperty("Assembler", value);
        }

        public string AssemblerOutput
        {
            get => UnconfiguredProject.MakeRooted(GetProperty("AssemblerOutput"));
            set => SetProperty("AssemblerOutput", UnconfiguredProject.MakeRelative(value));
        }

        public IReadOnlyList<string> AvailableOutputFormats => GetAvailableOutputFormats(Assembler);

        public string OutputFormat
        {
            get => GetProperty("OutputFormat");
            set => SetProperty("OutputFormat", value);
        }

        public ICommand BrowseAssemblerOutputCommand => new BrowseAssemblerOutputCommand(this, AssemblerOutput);

        private IReadOnlyList<string> GetAvailableOutputFormats(string aAssembler)
        {
            switch (aAssembler)
            {
                case AssemblerValues.NASM:
                    return ImmutableArray.Create<string>("Bin", "COFF", "ELF32", "ELF64", "Win32", "Win64");
                default:
                    return new string[0];
            }
        }
    }
    
    internal class BrowseAssemblerOutputCommand : ICommand
    {
        private AssemblePropertyPageViewModel mViewModel;
        private string mCurrentAssemblerOutput;

        public BrowseAssemblerOutputCommand(AssemblePropertyPageViewModel aViewModel, string aCurrentAssemblerOutput)
        {
            mViewModel = aViewModel;
            mCurrentAssemblerOutput = aCurrentAssemblerOutput;
        }

#pragma warning disable CS0067
        public event EventHandler CanExecuteChanged;
#pragma warning restore CS0067

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            var xSaveFileDialog = new SaveFileDialog
            {
                FileName = mCurrentAssemblerOutput
                // todo: add filter based on available output formats?
            };

            if (xSaveFileDialog.ShowDialog().GetValueOrDefault(false))
            {
                mViewModel.AssemblerOutput = xSaveFileDialog.FileName;
            }
        }
    }
}
