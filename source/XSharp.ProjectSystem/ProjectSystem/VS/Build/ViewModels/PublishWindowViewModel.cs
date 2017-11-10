using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.IO;
using System.Management;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using Microsoft.Win32;

using WinFormsDialogResult = System.Windows.Forms.DialogResult;
using FolderBrowserDialog = System.Windows.Forms.FolderBrowserDialog;

namespace XSharp.ProjectSystem.VS.Build
{
    internal class DefaultPublishProperties
    {
        public string IsoPublishPath { get; }

        public DefaultPublishProperties(string aIsoPublishPath = null)
        {
            IsoPublishPath = aIsoPublishPath;
        }
    }

    internal class PublishWindowViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private PublishWindow mPublishWindow;

        public PublishWindowViewModel(PublishWindow aPublishWindow, DefaultPublishProperties aDefaultProperties)
        {
            mPublishWindow = aPublishWindow;

            mIsoPublishPath = aDefaultProperties.IsoPublishPath;

            mBrowseIsoPublishPathCommand = new BrowseIsoPublishPathCommand(this, IsoPublishPath);

            mReturnPublishSettingsCommand = new ReturnPublishSettingsCommand(this);

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

        private void SetProperty<T>(ref T aPropertyRef, T aNewValue, [CallerMemberName]string aPropertyName = null)
        {
            if (!EqualityComparer<T>.Default.Equals(aPropertyRef, aNewValue))
            {
                aPropertyRef = aNewValue;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(aPropertyName));
            }
        }

        public IEnumerable<string> Drives => DriveInfo.GetDrives().Where(d => d.DriveType == DriveType.Removable)
                                                                  .Select(d => d.RootDirectory.FullName);

        private PublishType mPublishType;
        public PublishType PublishType
        {
            get => mPublishType;
            set => SetProperty(ref mPublishType, value);
        }

        private ICommand mBrowseIsoPublishPathCommand;
        public ICommand BrowseIsoPublishPathCommand
        {
            get => mBrowseIsoPublishPathCommand;
            set => SetProperty(ref mBrowseIsoPublishPathCommand, value);
        }

        private ICommand mReturnPublishSettingsCommand;
        public ICommand ReturnPublishSettingsCommand
        {
            get => mReturnPublishSettingsCommand;
            set => SetProperty(ref mReturnPublishSettingsCommand, value);
        }

        private string mIsoPublishPath;
        public string IsoPublishPath
        {
            get => mIsoPublishPath;
            set => SetProperty(ref mIsoPublishPath, value);
        }

        private string mUsbPublishDrive;
        public string UsbPublishDrive
        {
            get => mUsbPublishDrive;
            set => SetProperty(ref mUsbPublishDrive, value);
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
                default:
                    throw new NotImplementedException($"Publish type '{PublishType}' not implemented!");
            }

            return new PublishSettings(PublishType, xPublishPath, FormatUsbDrive);
        }

        public void ReturnPublishSettings()
        {
            mPublishWindow.ReturnPublishSettings();
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
                FileName = mInitialPath,
                Filter = "ISO Image (*.iso) | *.iso",
            };

            if (xSaveFileDialog.ShowDialog().GetValueOrDefault(false))
            {
                mViewModel.IsoPublishPath = xSaveFileDialog.FileName;
            }
        }
    }

    internal class ReturnPublishSettingsCommand : ICommand
    {
        private PublishWindowViewModel mViewModel;

        public ReturnPublishSettingsCommand(PublishWindowViewModel aViewModel)
        {
            mViewModel = aViewModel;
            mViewModel.PropertyChanged += delegate (object aSender, PropertyChangedEventArgs aEventArgs)
            {
                CanExecuteChanged(this, EventArgs.Empty);
            };
        }

        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            switch (mViewModel.PublishType)
            {
                case PublishType.ISO:
                    return Directory.Exists(Path.GetDirectoryName(mViewModel.IsoPublishPath));
                case PublishType.USB:
                    return !String.IsNullOrWhiteSpace(mViewModel.UsbPublishDrive);
                default:
                    return false;

            }
        }

        public void Execute(object parameter)
        {
            mViewModel.ReturnPublishSettings();
        }
    }
}
