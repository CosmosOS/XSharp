using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Windows.Input;
using Microsoft.VisualStudio.ProjectSystem;
using Microsoft.Win32;

using VSPropertyPages;

using static XSharp.ProjectSystem.ConfigurationGeneral;

namespace XSharp.ProjectSystem.VS.PropertyPages
{
    internal class AssemblePropertyPageViewModel : PropertyPageViewModel
    {
        public AssemblePropertyPageViewModel(IPropertyManager aPropertyManager, IProjectThreadingService aProjectThreadingService)
            : base(aPropertyManager, aProjectThreadingService)
        {
        }

        public bool Assemble
        {
            get => Boolean.Parse(GetProperty("Assemble"));
            set => SetProperty("Assemble", value.ToString(), nameof(Assemble));
        }

        public string Assembler
        {
            get => GetProperty("Assembler");
            set => SetProperty("Assembler", value, nameof(Assembler), nameof(AvailableOutputFormats));
        }

        public string AssemblerOutput
        {
            get => GetPathProperty("AssemblerOutput", false);
            set => SetPathProperty("AssemblerOutput", value, false, nameof(AssemblerOutput));
        }

        public IReadOnlyList<string> AvailableOutputFormats => GetAvailableOutputFormats(Assembler);

        public string OutputFormat
        {
            get => GetProperty("AssemblerOutputFormat");
            set => SetProperty("AssemblerOutputFormat", value, nameof(OutputFormat));
        }

        public ICommand BrowseAssemblerOutputCommand => new BrowseAssemblerOutputCommand(this, AssemblerOutput);

        private IReadOnlyList<string> GetAvailableOutputFormats(string aAssembler)
        {
            switch (aAssembler)
            {
                case AssemblerValues.NASM:
                    return ImmutableArray.Create("Bin", "COFF", "ELF32", "ELF64", "Win32", "Win64");
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
