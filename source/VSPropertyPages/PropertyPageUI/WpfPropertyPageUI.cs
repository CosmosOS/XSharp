using System;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Interop;
using Microsoft.VisualStudio;
using Microsoft.VisualStudio.OLE.Interop;
using Microsoft.VisualStudio.ProjectSystem;
using Microsoft.VisualStudio.Threading;

using MSG = Microsoft.VisualStudio.OLE.Interop.MSG;
using Message = System.Windows.Interop.MSG;

namespace VSPropertyPages
{
    public abstract class WpfPropertyPageUI : UserControl, IPropertyPageUI
    {
        private HwndSource _hWndSource;

        private IPropertyPageSite _propertyPageSite;
        private IProjectThreadingService _projectThreadingService;

        protected IProjectThreadingService ProjectThreadingService => _projectThreadingService;
        
        public Task SetPageSiteAsync(IPropertyPageSite site)
        {
            _propertyPageSite = site;
            return TplExtensions.CompletedTask;
        }

        public Task SetProjectThreadingServiceAsync(IProjectThreadingService projectThreadingService)
        {
            _projectThreadingService = projectThreadingService;
            return TplExtensions.CompletedTask;
        }

        public abstract Task SetViewModelAsync(PropertyPageViewModel propertyPageViewModel);

        public Task ActivateAsync(IntPtr hWndParent, Rectangle rect, bool modal)
        {
            var hWndSourceParams = new HwndSourceParameters
            {
                ParentWindow = hWndParent,
                PositionX = rect.Left,
                PositionY = rect.Top,
                Width = rect.Width,
                Height = rect.Height,
                WindowStyle = WS.WS_VISIBLE | WS.WS_CHILD
            };

            _hWndSource = new HwndSource(hWndSourceParams)
            {
                RootVisual = this,
                SizeToContent = SizeToContent.WidthAndHeight
            };

            return TplExtensions.CompletedTask;
        }

        public Task DeactivateAsync()
        {
            _hWndSource?.Dispose();
            return TplExtensions.CompletedTask;
        }

        public Task MoveAsync(Rectangle rect)
        {
            Margin = new Thickness(rect.Left, rect.Top, 0, 0);

            Width = rect.Width;
            Height = rect.Height;

            return TplExtensions.CompletedTask;
        }

        public Task ShowAsync(bool visible)
        {
            Visibility = visible ? Visibility.Visible : Visibility.Hidden;
            return TplExtensions.TrueTask;
        }

        public async Task<int> TranslateAcceleratorAsync(MSG msg)
        {
            var message = new Message
            {
                hwnd = msg.hwnd,
                message = (int)msg.message,
                wParam = msg.wParam,
                lParam = msg.lParam,
                time = (int)msg.time,
                pt_x = msg.pt.x,
                pt_y = msg.pt.y
            };

            var used = ComponentDispatcher.RaiseThreadMessage(ref message);

            if (used)
            {
                msg.message = (uint)message.message;
                msg.wParam = message.wParam;
                msg.lParam = message.lParam;

                return VSConstants.S_OK;
            }

            if (_propertyPageSite != null)
            {
                await _projectThreadingService.SwitchToUIThread();
                return _propertyPageSite.TranslateAccelerator(new MSG[] { msg });
            }

            return VSConstants.S_OK;
        }
    }
}
