using System;
using System.Windows;
using Microsoft.VisualStudio.PlatformUI;

namespace XSharp.ProjectSystem.VS.Build
{
    /// <summary>
    /// Interaction logic for PublishWindow.xaml
    /// </summary>
    internal partial class PublishWindow : DialogWindow
    {
        private PublishWindowViewModel mViewModel;

        public PublishWindow(DefaultPublishProperties aDefaultProperties)
        {
            InitializeComponent();
            Initialize(aDefaultProperties);
        }

        public PublishWindow(DefaultPublishProperties aDefaultProperties, string aHelpTopic) : base(aHelpTopic)
        {
            InitializeComponent();
            Initialize(aDefaultProperties);
        }

        private void Initialize(DefaultPublishProperties aDefaultProperties)
        {
            mViewModel = new PublishWindowViewModel(this, aDefaultProperties);
            DataContext = mViewModel;
        }

        public new PublishSettings ShowModal()
        {
            return base.ShowModal().GetValueOrDefault(false) ? mViewModel.ToPublishSettings() : null;
        }

        private void Cancel(object aSender, RoutedEventArgs aEventArgs)
        {
            DialogResult = false;
            Close();
        }

        public void ReturnPublishSettings()
        {
            if (mViewModel.PublishType == PublishType.USB && mViewModel.FormatUsbDrive)
            {
                MessageBox.Show($"The selected USB drive ({mViewModel.UsbPublishDrive}) will be formatted and its contents will be destroyed!{Environment.NewLine}Do you want to continue?",
                    "Publish", MessageBoxButton.YesNo, MessageBoxImage.Warning);
            }

            DialogResult = true;
            Close();
        }
    }
}
