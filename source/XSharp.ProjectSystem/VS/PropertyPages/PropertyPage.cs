using System;
using System.Collections.Generic;
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

using Message = System.Windows.Interop.MSG;
using Task = System.Threading.Tasks.Task;
using VsMsg = Microsoft.VisualStudio.OLE.Interop.MSG;

namespace XSharp.ProjectSystem.VS.PropertyPages
{
    public abstract class PropertyPage : UserControl, IPropertyPage
    {
        private const int WS_CHILD = 0x40000000;
        private const int WS_VISIBLE = 0x10000000;

        private IPropertyPageSite mPropertyPageSite;
        private bool mIsActivated = false;
        private bool mIsDirty = false;
        private HwndSource mHwndSource;

        protected IProjectThreadingService ThreadHandling;
        protected UnconfiguredProject UnconfiguredProject;

        protected abstract string PageName { get; }

        #region Private Methods

        private void WaitForAsync(Func<Task> aAsyncFunc)
        {
            ThreadHandling.ExecuteSynchronously(aAsyncFunc);
        }

        private T WaitForAsync<T>(Func<Task<T>> aAsyncFunc)
        {
            return ThreadHandling.ExecuteSynchronously(aAsyncFunc);
        }

        private void Move(RECT aRect)
        {
            Canvas.SetLeft(this, aRect.left);
            Canvas.SetTop(this, aRect.top);
            Canvas.SetRight(this, aRect.right);
            Canvas.SetBottom(this, aRect.bottom);
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

            //InvalidateVisual();
            //UpdateLayout();

            mIsActivated = true;
        }

        public void Deactivate()
        {
            if (mIsActivated)
            {
                WaitForAsync(OnDeactivate);
            }

            mIsActivated = false;
            mHwndSource.Dispose();
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

#pragma warning disable VSTHRD200

        protected abstract Task OnApply();
        protected abstract Task OnDeactivate();

#pragma warning restore VSTHRD200

        public void SetObjects(uint cObjects, object[] ppunk)
        {
            if (cObjects == 0)
            {
                return;
            }

            if (ppunk.Length < cObjects)
            {
                throw new ArgumentOutOfRangeException(nameof(cObjects));
            }

            List<string> configurations = new List<string>();

            ThreadHelper.ThrowIfNotOnUIThread();

            for (int i = 0; i < cObjects; ++i)
            {
                var xBrowseObject = ppunk[i] as IVsBrowseObject;

                if (xBrowseObject != null)
                {
                    int xHR = xBrowseObject.GetProjectItem(out var xHierarchy, out uint itemid);
                    
                    if (xHR == VSConstants.S_OK && itemid == VSConstants.VSITEMID_ROOT)
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
                                ThreadHandling = UnconfiguredProject.Services.ExportProvider.GetExportedValue<IProjectThreadingService>();
                            }
                        }
                    }
                }
            }
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

        public int IsPageDirty()
        {
            return mIsDirty ? VSConstants.S_OK : VSConstants.S_FALSE;
        }

        public int Apply()
        {
            WaitForAsync(OnApply);
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
                ThreadHelper.ThrowIfNotOnUIThread();
                xResult = mPropertyPageSite.TranslateAccelerator(pMsg);
            }

            return xResult;
        }

        #endregion
    }
}
