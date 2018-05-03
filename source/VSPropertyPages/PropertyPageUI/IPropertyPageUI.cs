using System;
using System.Drawing;
using System.Threading.Tasks;
using Microsoft.VisualStudio.OLE.Interop;
using Microsoft.VisualStudio.ProjectSystem;

namespace VSPropertyPages
{
    public interface IPropertyPageUI
    {
        Task SetProjectThreadingServiceAsync(IProjectThreadingService projectThreadingService);

        void Activate(IntPtr hWndParent, Rectangle rect, bool modal);
        void Deactivate();

        void Show(bool visible);
        void Move(Rectangle rect);
        bool TranslateAccelerator(ref MSG msg);
    }
}
