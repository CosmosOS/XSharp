using System;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.VisualStudio;
using Microsoft.VisualStudio.OLE.Interop;
using Microsoft.VisualStudio.ProjectSystem;
using Microsoft.VisualStudio.Threading;

namespace VSPropertyPages
{
    public class WinFormsPropertyPageUI : UserControl, IPropertyPageUI
    {
        private IPropertyPageSite _propertyPageSite;
        private IProjectThreadingService _projectThreadingService;
        
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

        public virtual Task SetViewModelAsync(PropertyPageViewModel propertyPageViewModel)
        {
            return Task.CompletedTask;
        }

        public Task ActivateAsync(IntPtr hWndParent, Rectangle rect, bool modal)
        {
            CreateControl();
            Win32Methods.SetParent(Handle, hWndParent);

            return TplExtensions.CompletedTask;
        }

        public Task DeactivateAsync()
        {
            Dispose();
            return TplExtensions.CompletedTask;
        }

        public Task ShowAsync(bool visible)
        {
            Visible = visible;
            return TplExtensions.CompletedTask;
        }

        public Task MoveAsync(Rectangle rect)
        {
            Location = rect.Location;
            Size = rect.Size;

            return TplExtensions.CompletedTask;
        }

        public Task<int> TranslateAcceleratorAsync(MSG msg)
        {
            if ((msg.message < WM.WM_KEYFIRST || msg.message > WM.WM_KEYLAST) &&
                (msg.message < WM.WM_MOUSEFIRST || msg.message > WM.WM_MOUSELAST))
            {
                return Task.FromResult(VSConstants.S_FALSE);
            }

            return Task.FromResult(Win32Methods.IsDialogMessageA(Handle, ref msg).ToVSConstant());
        }
    }
}
