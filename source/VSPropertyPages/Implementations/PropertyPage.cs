using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using Microsoft.VisualStudio;
using Microsoft.VisualStudio.ManagedInterfaces.ProjectDesigner;
using Microsoft.VisualStudio.OLE.Interop;
using Microsoft.VisualStudio.ProjectSystem;
using Microsoft.VisualStudio.ProjectSystem.Properties;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;

using Task = System.Threading.Tasks.Task;

namespace VSPropertyPages
{
    public abstract class PropertyPage : IPropertyPage2, IVsProjectDesignerPage
    {
        private IPropertyPageSite _propertyPageSite;
        private IVsProjectDesignerPageSite _vsProjectDesignerPageSite;

        private UnconfiguredProject _unconfiguredProject;
        private IProjectThreadingService _projectThreadingService;

        protected IPropertyPageSite PropertyPageSite => _propertyPageSite;
        protected IVsProjectDesignerPageSite VsProjectDesignerPageSite => _vsProjectDesignerPageSite;

        private IPropertyPageUI _propertyPageUI;
        private PropertyPageViewModel _propertyPageViewModel;

        public abstract string PageName { get; }

        public abstract IPropertyPageUI CreatePropertyPageUI();
        public abstract PropertyPageViewModel CreatePropertyPageViewModel(
            UnconfiguredProject unconfiguredProject, IProjectThreadingService projectThreadingService);

        private void WaitForAsync(Func<Task> asyncFunc) => _projectThreadingService.ExecuteSynchronously(asyncFunc);

        private T WaitForAsync<T>(Func<Task<T>> asyncFunc) => _projectThreadingService.ExecuteSynchronously(asyncFunc);

        private void PropertyChanging(object sender, ProjectPropertyChangingEventArgs e)
        {
            ThreadHelper.ThrowIfNotOnUIThread();
            _vsProjectDesignerPageSite.OnPropertyChanging(e.PropertyName, e.PropertyName.ToProjectPropertyDescriptor());
        }

        private void PropertyChanged(object sender, ProjectPropertyChangedEventArgs e)
        {
            ThreadHelper.ThrowIfNotOnUIThread();

            var isDirty = WaitForAsync(_propertyPageViewModel.IsDirtyAsync);

            _propertyPageSite.OnStatusChange(
                (uint)(isDirty ? PROPPAGESTATUS.PROPPAGESTATUS_DIRTY : PROPPAGESTATUS.PROPPAGESTATUS_CLEAN));
            _vsProjectDesignerPageSite.OnPropertyChanged(e.PropertyName, e.PropertyName.ToProjectPropertyDescriptor(),
                e.OldValue, e.NewValue);
        }

        #region IPropertyPage2

        public void SetPageSite(IPropertyPageSite pPageSite)
        {
            _propertyPageSite = pPageSite;
        }

        public void Activate(IntPtr hWndParent, RECT[] pRect, int bModal)
        {
#if DEBUG
            if (pRect == null || pRect.Length != 1)
            {
                throw new ArgumentOutOfRangeException(nameof(pRect));
            }
#endif
            _propertyPageUI = CreatePropertyPageUI();

            var vsRect = pRect[0];
            var rect = new Rectangle(vsRect.left, vsRect.top, vsRect.right - vsRect.left, vsRect.bottom - vsRect.top);

            var modal = bModal.ToBoolean();

            WaitForAsync(() => _propertyPageUI.ActivateAsync(hWndParent, rect, modal));
            WaitForAsync(() => _propertyPageUI.SetViewModelAsync(_propertyPageViewModel));
        }

        public void Deactivate() => WaitForAsync(_propertyPageUI.DeactivateAsync);

        public void GetPageInfo(PROPPAGEINFO[] pPageInfo)
        {
#if DEBUG
            if (pPageInfo == null)
            {
                throw new ArgumentNullException(nameof(pPageInfo));
            }

            if (pPageInfo.Length == 0)
            {
                throw new ArgumentOutOfRangeException(nameof(pPageInfo));
            }
#endif
            pPageInfo[0] = new PROPPAGEINFO()
            {
                cb = (uint)Marshal.SizeOf(typeof(PROPPAGEINFO)),
                dwHelpContext = 0,
                pszDocString = null,
                pszHelpFile = null,
                pszTitle = PageName,
                SIZE = new SIZE()
            };
        }

#pragma warning disable CA1725 // Parameter names should match base declaration
        public void SetObjects(uint cObjects, object[] ppUnk)
#pragma warning restore CA1725 // Parameter names should match base declaration
        {
            if (cObjects == 0)
            {
                return;
            }
#if DEBUG
            if (ppUnk == null)
            {
                throw new ArgumentNullException(nameof(ppUnk));
            }

            if (ppUnk.Length < cObjects)
            {
                throw new ArgumentOutOfRangeException(nameof(cObjects));
            }
#endif
            ThreadHelper.ThrowIfNotOnUIThread();

            for (int i = 0; i < cObjects; i++)
            {
                var unk = ppUnk[i];

                var context = unk as IVsBrowseObjectContext;

                if (context == null && unk is IVsBrowseObject browseObject)
                {
                    int hr = browseObject.GetProjectItem(out var hierarchy, out uint itemId);

                    if (hr == VSConstants.S_OK && itemId == VSConstants.VSITEMID_ROOT)
                    {
                        if (hierarchy != null)
                        {
                            if (ErrorHandler.Succeeded(hierarchy.GetProperty((uint)VSConstants.VSITEMID.Root, (int)__VSHPROPID.VSHPROPID_ExtObject, out var extObject)))
                            {
                                context = extObject as IVsBrowseObjectContext;

                                if (context == null)
                                {
                                    if (extObject is EnvDTE.Project dteProject)
                                    {
                                        context = dteProject.Object as IVsBrowseObjectContext;
                                    }
                                }
                            }
                        }
                    }
                }

                if (context != null)
                {
                    _unconfiguredProject = context.UnconfiguredProject;
                    _projectThreadingService = _unconfiguredProject.ProjectService.Services.ThreadingPolicy;
                }
            }

            if (_unconfiguredProject == null)
            {
                throw new Exception("Couldn't find UnconfiguredProject!");
            }

            _propertyPageViewModel = CreatePropertyPageViewModel(_unconfiguredProject, _projectThreadingService);
            
            _propertyPageViewModel.ProjectPropertyChanged += PropertyChanged;
            _propertyPageViewModel.ProjectPropertyChanging += PropertyChanging;
        }

        public void Show(uint nCmdShow)
        {
#if DEBUG
            if (!(nCmdShow == SW.SW_SHOW || nCmdShow == SW.SW_SHOWNORMAL || nCmdShow == SW.SW_HIDE))
            {
                throw new Exception("Unexpected nCmdShow value! nCmdShow = " + nCmdShow);
            }
#endif
            WaitForAsync(() => _propertyPageUI.ShowAsync(nCmdShow != SW.SW_HIDE));
        }

        public void Move(RECT[] pRect)
        {
#if DEBUG
            if (pRect == null || pRect.Length != 1)
            {
                throw new ArgumentOutOfRangeException(nameof(pRect));
            }
#endif
            var vsRect = pRect[0];
            var rect = new Rectangle(vsRect.left, vsRect.top, vsRect.right - vsRect.left, vsRect.bottom - vsRect.top);

            WaitForAsync(() => _propertyPageUI.MoveAsync(rect));
        }

        public int IsPageDirty() => _propertyPageViewModel == null ? VSConstants.S_FALSE
           : WaitForAsync(_propertyPageViewModel.IsDirtyAsync).ToVSConstant();

        public void Apply() => WaitForAsync(_propertyPageViewModel.ApplyAsync);

        public void Help(string pszHelpDir)
        {
            throw new NotImplementedException();
        }

        public int TranslateAccelerator(MSG[] pMsg)
        {
#if DEBUG
            if (pMsg == null)
            {
                throw new ArgumentNullException(nameof(pMsg));
            }

            if (pMsg.Length != 1)
            {
                throw new ArgumentOutOfRangeException(nameof(pMsg));
            }
#endif
            return WaitForAsync(() => _propertyPageUI.TranslateAcceleratorAsync(pMsg[0]));
        }

        public void EditProperty(int DISPID) => throw new NotImplementedException();

        int IPropertyPage.Apply() => WaitForAsync(_propertyPageViewModel.ApplyAsync).ToVSConstant();

        #endregion

        #region IVsProjectDesignerPage

        public void SetSite(IVsProjectDesignerPageSite site)
        {
            _vsProjectDesignerPageSite = site;
        }

        public object GetProperty(string propertyName) =>
            WaitForAsync(() => _propertyPageViewModel.GetPropertyAsync(propertyName));

        public void SetProperty(string propertyName, object value) =>
            WaitForAsync(() => _propertyPageViewModel.SetPropertyAsync(propertyName, (string)value));

        public bool SupportsMultipleValueUndo(string propertyName) => false;

        public bool GetPropertyMultipleValues(string propertyName, out object[] objects, out object[] values) =>
            throw new NotImplementedException();

        public void SetPropertyMultipleValues(string propertyName, object[] objects, object[] values) =>
            throw new NotImplementedException();

        public bool FinishPendingValidations() => true;

        public void OnActivated(bool activated) { }

        #endregion
    }
}
