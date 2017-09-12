using System;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Interop;
using Microsoft.VisualStudio;
using Microsoft.VisualStudio.OLE.Interop;
using Microsoft.VisualStudio.ProjectSystem;
using Microsoft.VisualStudio.ProjectSystem.Properties;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;

using IServiceProvider = System.IServiceProvider;
using Message = System.Windows.Interop.MSG;
using Task = System.Threading.Tasks.Task;
using VsMsg = Microsoft.VisualStudio.OLE.Interop.MSG;

namespace XSharp.ProjectSystem.VS.PropertyPages
{
    #region Designer Support

    // code from: https://stackoverflow.com/a/17661386/4647866

    public class AbstractControlDescriptionProvider<TAbstract, TBase> : TypeDescriptionProvider
    {
        public AbstractControlDescriptionProvider()
            : base(TypeDescriptor.GetProvider(typeof(TAbstract)))
        {
        }

        public override Type GetReflectionType(Type objectType, object instance)
        {
            if (objectType == typeof(TAbstract))
            {
                return typeof(TBase);
            }

            return base.GetReflectionType(objectType, instance);
        }

        public override object CreateInstance(IServiceProvider provider, Type objectType, Type[] argTypes, object[] args)
        {
            if (objectType == typeof(TAbstract))
            {
                objectType = typeof(TBase);
            }

            return base.CreateInstance(provider, objectType, argTypes, args);
        }
    }

    #endregion

    [TypeDescriptionProvider(typeof(AbstractControlDescriptionProvider<PropertyPage, UserControl>))]
    public abstract class PropertyPage : UserControl, IPropertyPage
    {
        private const int WS_CHILD = 0x40000000;
        private const int WS_VISIBLE = 0x10000000;

        private IPropertyPageSite mPropertyPageSite;
        private bool mIsActivated = false;
        private HwndSource mHwndSource;

        protected IProjectThreadingService ProjectThreadingService;
        protected UnconfiguredProject UnconfiguredProject;

        protected abstract string PageName { get; }
        protected abstract PropertyPageViewModel ViewModel { get; }

        protected abstract void SetObjects(UnconfiguredProject aUnconfiguredProject);

        #region Private Methods

        private void WaitForAsync(Func<Task> aAsyncFunc)
        {
            ProjectThreadingService.ExecuteSynchronously(aAsyncFunc);
        }

        private T WaitForAsync<T>(Func<Task<T>> aAsyncFunc)
        {
            return ProjectThreadingService.ExecuteSynchronously(aAsyncFunc);
        }

        private void Move(RECT aRect)
        {
            // todo: is this needed? it looks like left and top are always 0
            //VisualOffset = new Vector(aRect.left, aRect.top);
            Width = aRect.right - aRect.left;
            Height = aRect.bottom - aRect.top;
        }
        
        #endregion
        
        #region IPropertyPage

        public void SetPageSite(IPropertyPageSite pPageSite)
        {
            mPropertyPageSite = pPageSite;
        }

        public void Activate(IntPtr hWndParent, RECT[] pRect, int bModal)
        {
            if (pRect == null || pRect.Length == 0)
            {
                throw new ArgumentNullException(nameof(pRect));
            }

            var xRect = pRect[0];
            var xParams = new HwndSourceParameters();

            xParams.ParentWindow = hWndParent;
            xParams.PositionX = xRect.left;
            xParams.PositionY = xRect.top;
            xParams.Width = xRect.right - xRect.left;
            xParams.Height = xRect.bottom - xRect.top;
            xParams.WindowStyle = WS_VISIBLE | WS_CHILD;

            mHwndSource = new HwndSource(xParams);
            mHwndSource.RootVisual = this;
            mHwndSource.SizeToContent = SizeToContent.WidthAndHeight;
            
            mIsActivated = true;
        }

        public void Deactivate()
        {
            if (mIsActivated)
            {
                mHwndSource.Dispose();
                ViewModel.Dispose();
            }

            mIsActivated = false;
        }

        public void GetPageInfo(PROPPAGEINFO[] pPageInfo)
        {
            var xInfo = new PROPPAGEINFO();

            xInfo.cb = (uint)Marshal.SizeOf(typeof(PROPPAGEINFO));
            xInfo.dwHelpContext = 0;
            xInfo.pszDocString = null;
            xInfo.pszHelpFile = null;
            xInfo.pszTitle = PageName;
            xInfo.SIZE.cx = 0;
            xInfo.SIZE.cy = 0;

            if (pPageInfo != null && pPageInfo.Length > 0)
            {
                pPageInfo[0] = xInfo;
            }
        }
        
        public void SetObjects(uint cObjects, object[] ppUnk)
        {
            if (cObjects == 0)
            {
                return;
            }

            if (ppUnk.Length < cObjects)
            {
                throw new ArgumentOutOfRangeException(nameof(cObjects));
            }

            ThreadHelper.ThrowIfNotOnUIThread();

            for (int i = 0; i < cObjects; i++)
            {
                var xBrowseObject = ppUnk[i] as IVsBrowseObject;

                if (xBrowseObject != null)
                {
                    int xHR = xBrowseObject.GetProjectItem(out var xHierarchy, out uint xItemId);
                    
                    if (xHR == VSConstants.S_OK && xItemId == VSConstants.VSITEMID_ROOT)
                    {
                        if (xHierarchy != null)
                        {
                            if (ErrorHandler.Succeeded(xHierarchy.GetProperty((uint)VSConstants.VSITEMID.Root, (int)__VSHPROPID.VSHPROPID_ExtObject, out var xExtObject)))
                            {
                                var xContext = xExtObject as IVsBrowseObjectContext;

                                if (xContext == null)
                                {
                                    if (xExtObject is EnvDTE.Project xDteProject)
                                    {
                                        xContext = xDteProject.Object as IVsBrowseObjectContext;

                                        if (xContext == null)
                                        {
                                            throw new Exception("Couldn't find UnconfiguredProject!");
                                        }
                                    }
                                }

                                UnconfiguredProject = xContext.UnconfiguredProject;
                                ProjectThreadingService = UnconfiguredProject.ProjectService.Services.ThreadingPolicy;
                            }
                        }
                    }
                }
            }

            SetObjects(UnconfiguredProject);
            DataContext = ViewModel;
        }

        public void Show(uint nCmdShow)
        {
            if (nCmdShow == 0)
            {
                Visibility = Visibility.Hidden;
            }
            else
            {
                Visibility = Visibility.Visible;
            }
        }

        public void Move(RECT[] pRect)
        {
            if (pRect == null || pRect.Length <= 0)
            {
                throw new ArgumentNullException(nameof(pRect));
            }

            RECT xRect = pRect[0];
            Move(xRect);
        }

        public int IsPageDirty() => (ViewModel?.PropertiesChanged).GetValueOrDefault(false) ? VSConstants.S_OK : VSConstants.S_FALSE;

        public int Apply()
        {
            WaitForAsync(ViewModel.ApplyAsync);
            return VSConstants.S_OK;
        }

        public void Help(string pszHelpDir)
        {
            throw new NotImplementedException();
        }

        public int TranslateAccelerator(VsMsg[] pMsg)
        {
            if (pMsg == null || pMsg.Length == 0)
            {
                throw new ArgumentNullException(nameof(pMsg));
            }

            var xMsg = pMsg[0];
            var xMessage = new Message();

            xMessage.hwnd = xMsg.hwnd;
            xMessage.message = (int)xMsg.message;
            xMessage.wParam = xMsg.wParam;
            xMessage.lParam = xMsg.lParam;
            xMessage.time = (int)xMsg.time;
            xMessage.pt_x = xMsg.pt.x;
            xMessage.pt_y = xMsg.pt.y;

            var xUsed = ComponentDispatcher.RaiseThreadMessage(ref xMessage);
            
            if (xUsed)
            {
                xMsg.message = (uint)xMessage.message;
                xMsg.wParam = xMessage.wParam;
                xMsg.lParam = xMessage.lParam;

                return VSConstants.S_OK;
            }

            int xResult = 0;
            if (mPropertyPageSite != null)
            {
                ProjectThreadingService.SwitchToUIThread();
                xResult = mPropertyPageSite.TranslateAccelerator(pMsg);
            }

            return xResult;
        }

        #endregion
    }
}
