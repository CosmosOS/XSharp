using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Management;
using System.Windows;
using Microsoft.VisualStudio.PlatformUI;
using Microsoft.Win32;
using WinFormsDialogResult = System.Windows.Forms.DialogResult;
using FolderBrowserDialog = System.Windows.Forms.FolderBrowserDialog;

namespace XSharp.ProjectSystem.VS.Build
{
    /// <summary>
    /// Interaction logic for PublishWindow.xaml
    /// </summary>
    public partial class PublishWindow : DialogWindow
    {
        private PublishWindowViewModel mViewModel;

        public PublishWindow()
        {
            InitializeComponent();
            Initialize();
        }

        public PublishWindow(string aHelpTopic) : base(aHelpTopic)
        {
            InitializeComponent();
            Initialize();
        }

        private void Initialize()
        {
            mViewModel = new PublishWindowViewModel();
            DataContext = mViewModel;
        }

        public new PublishSettings ShowModal()
        {
            return base.ShowModal().GetValueOrDefault(false) ? mViewModel.ToPublishSettings() : null;
        }

        private void ReturnPublishSettings(object aSender, RoutedEventArgs aEventArgs)
        {
            if (mViewModel.PublishType == PublishType.USB && mViewModel.FormatUsbDrive)
            {
                MessageBox.Show($"The selected USB drive ({mViewModel.UsbPublishDrive}) will be formatted and its contents will be destroyed!{Environment.NewLine}Do you want to continue?",
                    "Publish", MessageBoxButton.YesNo, MessageBoxImage.Warning);
            }

            DialogResult = true;
            Close();
        }

        private void Cancel(object aSender, RoutedEventArgs aEventArgs)
        {
            Close();
        }

        private void BrowseIsoPublishPath(object aSender, RoutedEventArgs aEventArgs)
        {
            var xSaveFileDialog = new SaveFileDialog();

            xSaveFileDialog.Filter = "ISO Image (*.iso) | *.iso";
            xSaveFileDialog.InitialDirectory = Directory.GetCurrentDirectory();

            if (xSaveFileDialog.ShowDialog().GetValueOrDefault(false))
            {
                mViewModel.IsoPublishPath = xSaveFileDialog.FileName;
            }
        }

        private void BrowsePxePublishPath(object aSender, RoutedEventArgs aEventArgs)
        {
            var xFolderBrowserDialog = new FolderBrowserDialog();

            xFolderBrowserDialog.SelectedPath = Directory.GetCurrentDirectory();
            xFolderBrowserDialog.ShowDialog();

            if (xFolderBrowserDialog.ShowDialog() == WinFormsDialogResult.OK)
            {
                mViewModel.PxePublishPath = xFolderBrowserDialog.SelectedPath;
            }
        }
    }

    public class PublishWindowViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        // todo: get default property values from project?
        public PublishWindowViewModel()
        {
            WqlEventQuery xDeviceInsertedQuery = new WqlEventQuery("SELECT * FROM __InstanceCreationEvent WITHIN 2 WHERE TargetInstance ISA 'Win32_USBHub'");
            var xDeviceInsertedWatcher = new ManagementEventWatcher(xDeviceInsertedQuery);
            xDeviceInsertedWatcher.EventArrived += DrivesChanged;
            xDeviceInsertedWatcher.Start();

            WqlEventQuery xDeviceRemovedQuery = new WqlEventQuery("SELECT * FROM __InstanceDeletionEvent WITHIN 2 WHERE TargetInstance ISA 'Win32_USBHub'");
            var xDeviceRemovedWatcher = new ManagementEventWatcher(xDeviceRemovedQuery);
            xDeviceRemovedWatcher.EventArrived += DrivesChanged;
            xDeviceRemovedWatcher.Start();
        }

        private void DrivesChanged(object aSender, EventArrivedEventArgs aEventArgs)
        {
            PropertyChanged(this, new PropertyChangedEventArgs(nameof(Drives)));
        }

        private void SetProperty<T>(ref T aPropertyRef, T aNewValue, string aPropertyName)
        {
            if (EqualityComparer<T>.Default.Equals(aPropertyRef, aNewValue))
            {
                aPropertyRef = aNewValue;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(aPropertyName));
            }
        }

        public IEnumerable<string> Drives
        {
            get
            {
                return DriveInfo.GetDrives().Where(d => d.DriveType == DriveType.Removable).Select(d => d.RootDirectory.FullName);
            }
        }

        private PublishType mPublishType;
        public PublishType PublishType
        {
            get => mPublishType;
            set => SetProperty(ref mPublishType, value, nameof(PublishType));
        }

        // todo: use project properties
        private string mIsoPublishPath = Path.Combine(Directory.GetCurrentDirectory(), "Publish.iso");
        public string IsoPublishPath
        {
            get => mIsoPublishPath;
            set => SetProperty(ref mIsoPublishPath, value, nameof(IsoPublishPath));
        }

        private string mUsbPublishDrive;
        public string UsbPublishDrive
        {
            get => mUsbPublishDrive;
            set => SetProperty(ref mUsbPublishDrive, value, nameof(UsbPublishDrive));
        }

        private string mPxePublishPath;
        public string PxePublishPath
        {
            get => mPxePublishPath;
            set => SetProperty(ref mPxePublishPath, value, nameof(PxePublishPath));
        }

        // todo: format usb drive should be true by default?
        private bool mFormatUsbDrive = false;
        public bool FormatUsbDrive
        {
            get => mFormatUsbDrive;
            set => SetProperty(ref mFormatUsbDrive, value, nameof(FormatUsbDrive));
        }

        public PublishSettings ToPublishSettings()
        {
            string xPublishPath;

            switch (PublishType)
            {
                case PublishType.ISO:
                    xPublishPath = IsoPublishPath;
                    break;
                case PublishType.USB:
                    xPublishPath = UsbPublishDrive;
                    break;
                case PublishType.PXE:
                    xPublishPath = PxePublishPath;
                    break;
                default:
                    throw new NotImplementedException($"Publish type '{PublishType}' not implemented!");
            }

            return new PublishSettings(PublishType, xPublishPath, FormatUsbDrive);
        }
    }
}
