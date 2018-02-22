using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
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
    public abstract class PropertyPageBase : IPropertyPage2, IVsProjectDesignerPage
    {
        public abstract string PageName { get; }

        public abstract IPropertyPageUI CreatePropertyPageUI();
        public abstract IPropertyManager CreatePropertyManager(IReadOnlyCollection<ConfiguredProject> configuredProjects);

        protected IPropertyPageSite PropertyPageSite => _propertyPageSite;
        protected IVsProjectDesignerPageSite VsProjectDesignerPageSite => _vsProjectDesignerPageSite;

        protected UnconfiguredProject UnconfiguredProject => _unconfiguredProject;
        protected IProjectThreadingService ProjectThreadingService => _projectThreadingService;

        protected IPropertyManager PropertyManager => _propertyManager;

        private IPropertyPageSite _propertyPageSite;
        private IVsProjectDesignerPageSite _vsProjectDesignerPageSite;

        private UnconfiguredProject _unconfiguredProject;
        private IProjectThreadingService _projectThreadingService;

        private IPropertyPageUI _propertyPageUI;
        private IPropertyManager _propertyManager;

        #region IPropertyPage2

        public void SetPageSite(IPropertyPageSite pPageSite) => _propertyPageSite = pPageSite;

        public void Activate(IntPtr hWndParent, RECT[] pRect, int bModal)
        {
#if DEBUG
            if (pRect == null)
            {
                throw new ArgumentNullException(nameof(pRect));
            }

            if (pRect.Length == 0)
            {
                throw new ArgumentOutOfRangeException(nameof(pRect));
            }
#endif
            _propertyPageUI = CreatePropertyPageUI();

            var vsRect = pRect[0];
            var rect = new Rectangle(vsRect.left, vsRect.top, vsRect.right - vsRect.left, vsRect.bottom - vsRect.top);

            var modal = bModal.ToBoolean();

            _propertyPageUI.Activate(hWndParent, rect, modal);
        }

        public void Deactivate() => _propertyPageUI.Deactivate();

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

            var configuredProjects = new List<ConfiguredProject>((int)cObjects);

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
                    configuredProjects.Add(context.ConfiguredProject);
                }
            }

#if DEBUG
            if (!configuredProjects.Any())
            {
                throw new InvalidOperationException("cObjects > 0, but no configurations were found!");
            }
#endif

            _unconfiguredProject = configuredProjects.First().UnconfiguredProject;
            _projectThreadingService = _unconfiguredProject.ProjectService.Services.ThreadingPolicy;

            if (_propertyManager == null)
            {
                _propertyManager = CreatePropertyManager(configuredProjects);

                _propertyManager.PropertyChanged += PropertyChanged;
                _propertyManager.PropertyChanging += PropertyChanging;
            }
            else
            {
                WaitForAsync(() => _propertyManager.UpdateConfigurationsAsync(configuredProjects));
            }
        }

        public void Show(uint nCmdShow)
        {
#if DEBUG
            if (!(nCmdShow == SW.SW_SHOW || nCmdShow == SW.SW_SHOWNORMAL || nCmdShow == SW.SW_HIDE))
            {
                throw new Exception("Unexpected nCmdShow value! nCmdShow = " + nCmdShow);
            }
#endif
            _propertyPageUI.Show(nCmdShow != SW.SW_HIDE);
        }

        public void Move(RECT[] pRect)
        {
#if DEBUG
            if (pRect == null)
            {
                throw new ArgumentNullException(nameof(pRect));
            }

            if (pRect.Length == 0)
            {
                throw new ArgumentOutOfRangeException(nameof(pRect));
            }
#endif
            var vsRect = pRect[0];
            var rect = new Rectangle(vsRect.left, vsRect.top, vsRect.right - vsRect.left, vsRect.bottom - vsRect.top);

            _propertyPageUI.Move(rect);
        }

        public int IsPageDirty() => _propertyManager == null ? VSConstants.S_FALSE
           : WaitForAsync(_propertyManager.IsDirtyAsync).ToVSConstant();

        public void Apply() => WaitForAsync(_propertyManager.ApplyAsync);

        public void Help(string pszHelpDir) => throw new NotImplementedException();

        public int TranslateAccelerator(MSG[] pMsg)
        {
#if DEBUG
            if (pMsg.Length == 0)
            {
                throw new ArgumentOutOfRangeException(nameof(pMsg));
            }
#endif
            if (pMsg == null)
            {
                return VSConstants.E_POINTER;
            }

            if (_propertyPageUI.TranslateAccelerator(ref pMsg[0]))
            {
                return VSConstants.S_OK;
            }
            
            _projectThreadingService.VerifyOnUIThread();
            return _propertyPageSite?.TranslateAccelerator(pMsg) ?? VSConstants.S_FALSE;
        }

        public void EditProperty(int DISPID) => throw new NotImplementedException();

        int IPropertyPage.Apply() => WaitForAsync(_propertyManager.ApplyAsync).ToVSConstant();

        #endregion

        #region IVsProjectDesignerPage

        public void SetSite(IVsProjectDesignerPageSite site) => _vsProjectDesignerPageSite = site;

        public object GetProperty(string propertyName) =>
            WaitForAsync(() => _propertyManager.GetPropertyAsync(propertyName));

        public void SetProperty(string propertyName, object value) =>
            WaitForAsync(() => _propertyManager.SetPropertyAsync(propertyName, (string)value));

        public bool SupportsMultipleValueUndo(string propertyName) => false;

        public bool GetPropertyMultipleValues(string propertyName, out object[] objects, out object[] values) =>
            throw new NotImplementedException();

        public void SetPropertyMultipleValues(string propertyName, object[] objects, object[] values) =>
            throw new NotImplementedException();

        public bool FinishPendingValidations() => true;

        public void OnActivated(bool activated) { }

        #endregion

        private void WaitForAsync(Func<Task> asyncFunc) => _projectThreadingService.ExecuteSynchronously(asyncFunc);

        private T WaitForAsync<T>(Func<Task<T>> asyncFunc) => _projectThreadingService.ExecuteSynchronously(asyncFunc);

        private void PropertyChanging(object sender, ProjectPropertyChangingEventArgs e)
        {
            _projectThreadingService.VerifyOnUIThread();
            _vsProjectDesignerPageSite.OnPropertyChanging(e.PropertyName, e.PropertyName.ToProjectPropertyDescriptor());
        }

        private void PropertyChanged(object sender, ProjectPropertyChangedEventArgs e)
        {
            _projectThreadingService.VerifyOnUIThread();

            var isDirty = WaitForAsync(_propertyManager.IsDirtyAsync);

            _propertyPageSite.OnStatusChange(
                (uint)(isDirty ? PROPPAGESTATUS.PROPPAGESTATUS_DIRTY : PROPPAGESTATUS.PROPPAGESTATUS_CLEAN));
            _vsProjectDesignerPageSite.OnPropertyChanged(
                e.PropertyName, e.PropertyName.ToProjectPropertyDescriptor(), e.OldValue, e.NewValue);
        }
    }
}
