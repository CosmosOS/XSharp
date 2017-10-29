using System;
using System.Drawing;
using System.Threading.Tasks;
using Microsoft.VisualStudio.OLE.Interop;
using Microsoft.VisualStudio.ProjectSystem;

namespace VSPropertyPages
{
    public interface IPropertyPageUI
    {
        Task SetPageSiteAsync(IPropertyPageSite site);
        Task SetProjectThreadingServiceAsync(IProjectThreadingService projectThreadingService);
        Task SetViewModelAsync(PropertyPageViewModel propertyPageViewModel);

        Task ActivateAsync(IntPtr hWndParent, Rectangle rect, bool modal);
        Task DeactivateAsync();

        Task ShowAsync(bool visible);
        Task MoveAsync(Rectangle rect);
        Task<int> TranslateAcceleratorAsync(MSG msg);
    }
}
