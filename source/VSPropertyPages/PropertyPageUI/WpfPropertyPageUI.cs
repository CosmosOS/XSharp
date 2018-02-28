using System;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Interop;
using Microsoft.VisualStudio.ProjectSystem;

using MSG = Microsoft.VisualStudio.OLE.Interop.MSG;
using Message = System.Windows.Interop.MSG;

namespace VSPropertyPages
{
    public class WpfPropertyPageUI : UserControl, IPropertyPageUI, IDisposable
    {
        private HwndSource _hWndSource;

        private IProjectThreadingService _projectThreadingService;

        public Task SetProjectThreadingServiceAsync(IProjectThreadingService projectThreadingService)
        {
            _projectThreadingService = projectThreadingService;
            return Task.CompletedTask;
        }

        public void Activate(IntPtr hWndParent, Rectangle rect, bool modal)
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
        }

        public void Deactivate() => Dispose();

        public void Move(Rectangle rect)
        {
            Margin = new Thickness(rect.Left, rect.Top, 0, 0);

            Width = rect.Width;
            Height = rect.Height;
        }

        public void Show(bool visible) => Visibility = visible ? Visibility.Visible : Visibility.Hidden;

        public bool TranslateAccelerator(ref MSG msg)
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

                return true;
            }

            return false;
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                _hWndSource?.Dispose();
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
