using System;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.VisualStudio;
using Microsoft.VisualStudio.OLE.Interop;
using Microsoft.VisualStudio.ProjectSystem;

namespace VSPropertyPages
{
    public class WinFormsPropertyPageUI : UserControl, IPropertyPageUI
    {
        private IProjectThreadingService _projectThreadingService;

        public Task SetProjectThreadingServiceAsync(IProjectThreadingService projectThreadingService)
        {
            _projectThreadingService = projectThreadingService;
            return Task.CompletedTask;
        }

        public void Activate(IntPtr hWndParent, Rectangle rect, bool modal)
        {
            CreateControl();
            Win32Methods.SetParent(Handle, hWndParent);
        }

        public void Deactivate() => Dispose();
        public void Show(bool visible) => Visible = visible;

        void IPropertyPageUI.Move(Rectangle rect)
        {
            Location = rect.Location;
            Size = rect.Size;
        }

        public bool TranslateAccelerator(ref MSG msg)
        {
            var message = Message.Create(msg.hwnd, (int)msg.message, msg.wParam, msg.lParam);
            
            var target = FromChildHandle(message.HWnd);
            var used = target?.PreProcessMessage(ref message) ?? false;

            if (used)
            {
                msg.message = (uint)message.Msg;
                msg.wParam = message.WParam;
                msg.lParam = message.LParam;

                return true;
            }

            return false;
        }
    }
}
