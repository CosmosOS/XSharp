using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Management;
using System.Windows;
using System.Windows.Input;
using Microsoft.VisualStudio.PlatformUI;
using Microsoft.Win32;
using WinFormsDialogResult = System.Windows.Forms.DialogResult;
using FolderBrowserDialog = System.Windows.Forms.FolderBrowserDialog;

namespace XSharp.ProjectSystem.VS.Build
{
    public class DefaultPublishProperties
    {
        public string IsoPublishPath { get; }
        public string PxePublishPath { get; }

        public DefaultPublishProperties(string aIsoPublishPath = null, string aPxePublishPath = null)
        {
            IsoPublishPath = aIsoPublishPath;
            PxePublishPath = aPxePublishPath;
        }
    }

    /// <summary>
    /// Interaction logic for PublishWindow.xaml
    /// </summary>
    public partial class PublishWindow : DialogWindow
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
            mViewModel = new PublishWindowViewModel(aDefaultProperties);
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
            DialogResult = false;
            Close();
        }
    }

    internal class PublishWindowViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        
        public PublishWindowViewModel(DefaultPublishProperties aDefaultProperties)
        {
            mIsoPublishPath = aDefaultProperties.IsoPublishPath;
            mPxePublishPath = aDefaultProperties.PxePublishPath;

            mBrowseIsoPublishPathCommand = new BrowseIsoPublishPathCommand(this, Path.GetDirectoryName(aDefaultProperties.IsoPublishPath));
            mBrowsePxePublishPathCommand = new BrowsePxePublishPathCommand(this, Path.GetDirectoryName(aDefaultProperties.PxePublishPath));

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
            if (!EqualityComparer<T>.Default.Equals(aPropertyRef, aNewValue))
            {
                aPropertyRef = aNewValue;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(aPropertyName));
            }

            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(EnableOK)));
        }

        private bool ValidateInput()
        {
            switch (PublishType)
            {
                case PublishType.ISO:
                    return Directory.Exists(Path.GetDirectoryName(mIsoPublishPath));
                case PublishType.USB:
                    return !String.IsNullOrWhiteSpace(mUsbPublishDrive);
                case PublishType.PXE:
                    return Directory.Exists(Path.GetDirectoryName(mPxePublishPath));
                default:
                    return false;

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

        private ICommand mBrowseIsoPublishPathCommand;
        public ICommand BrowseIsoPublishPathCommand
        {
            get
            {
                return mBrowseIsoPublishPathCommand;
            }
            set
            {
                SetProperty(ref mBrowseIsoPublishPathCommand, value, nameof(BrowseIsoPublishPathCommand));
            }
        }

        private ICommand mBrowsePxePublishPathCommand;
        public ICommand BrowsePxePublishPathCommand
        {
            get
            {
                return mBrowsePxePublishPathCommand;
            }
            set
            {
                SetProperty(ref mBrowsePxePublishPathCommand, value, nameof(BrowsePxePublishPathCommand));
            }
        }

        private string mIsoPublishPath;
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

        public bool EnableOK => ValidateInput();

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

    internal class BrowseIsoPublishPathCommand : ICommand
    {
        private PublishWindowViewModel mViewModel;
        private string mInitialPath;

        public BrowseIsoPublishPathCommand(PublishWindowViewModel aViewModel, string aInitialPath)
        {
            mViewModel = aViewModel;
            mInitialPath = aInitialPath;
        }

        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            var xSaveFileDialog = new SaveFileDialog();

            xSaveFileDialog.Filter = "ISO Image (*.iso) | *.iso";
            xSaveFileDialog.InitialDirectory = mInitialPath;

            if (xSaveFileDialog.ShowDialog().GetValueOrDefault(false))
            {
                mViewModel.IsoPublishPath = xSaveFileDialog.FileName;
            }
        }
    }

    internal class BrowsePxePublishPathCommand : ICommand
    {
        private PublishWindowViewModel mViewModel;
        private string mInitialPath;

        public BrowsePxePublishPathCommand(PublishWindowViewModel aViewModel, string aInitialPath)
        {
            mViewModel = aViewModel;
            mInitialPath = aInitialPath;
        }

        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            var xFolderBrowserDialog = new FolderBrowserDialog();

            xFolderBrowserDialog.SelectedPath = mInitialPath;
            xFolderBrowserDialog.ShowDialog();

            if (xFolderBrowserDialog.ShowDialog() == WinFormsDialogResult.OK)
            {
                mViewModel.PxePublishPath = xFolderBrowserDialog.SelectedPath;
            }
        }
    }
}
