using System;
using System.ComponentModel.Design;
using System.Runtime.InteropServices;
using Microsoft.VisualStudio.OLE.Interop;
using Microsoft.VisualStudio.Package;
using Microsoft.VisualStudio.Shell;

namespace XSharp.VS
{
    [Guid(PackageGuid)]
    [PackageRegistration(UseManagedResourcesOnly = true)]
    [InstalledProductRegistration("#110", "#112", "1.0", IconResourceID = 400)]
    [ProvideService(typeof(XSharpLanguageService))]
    [ProvideLanguageExtension(typeof(XSharpLanguageService), ".xs")]
    [ProvideLanguageService(typeof(XSharpLanguageService), "X#", 0, RequestStockColors = true)]
    internal sealed class XSharpPackage : Package, IOleComponent
    {
        /// <summary>
        /// The GUID for this package.
        /// </summary>
        public const string PackageGuid = "e2ce86d3-fb0b-43ad-938a-5bcdd087ea2d";

        private uint mComponentID;

        #region IOleComponent

        public int FReserved1(uint dwReserved, uint message, IntPtr wParam, IntPtr lParam) => 1;
        public int FPreTranslateMessage(MSG[] pMsg) => 0;
        public void OnEnterState(uint uStateID, int fEnter) { }
        public void OnAppActivate(int fActive, uint dwOtherThreadID) { }
        public void OnLoseActivation() { }
        public void OnActivationChange(IOleComponent pic, int fSameComponent, OLECRINFO[] pcrinfo, int fHostIsActivating, OLECHOSTINFO[] pchostinfo, uint dwReserved) { }

        public int FDoIdle(uint grfidlef)
        {
            bool bPeriodic = (grfidlef & (uint)_OLEIDLEF.oleidlefPeriodic) != 0;
            if (GetService(typeof(XSharpLanguageService)) is LanguageService xService)
            {
                xService.OnIdle(bPeriodic);
            }
            return 0;
        }

        public int FContinueMessageLoop(uint uReason, IntPtr pvLoopData, MSG[] pMsgPeeked) => 1;
        public int FQueryTerminate(int fPromptUser) => 1;
        public void Terminate() { }
        public IntPtr HwndGetWindow(uint dwWhich, uint dwReserved) => IntPtr.Zero;

        #endregion

        protected override void Initialize()
        {
            base.Initialize();

            // Proffer the service.
            var serviceContainer = this as IServiceContainer;
            var langService = new XSharpLanguageService();
            langService.SetSite(this);
            serviceContainer.AddService(typeof(XSharpLanguageService), langService, true);

            // Register a timer to call our language service during idle periods.
            if (mComponentID == 0 && (GetService(typeof(SOleComponentManager)) is IOleComponentManager xMgr))
            {
                var crinfo = new OLECRINFO
                {
                    cbSize = (uint)Marshal.SizeOf(typeof(OLECRINFO)),
                    grfcrf = (uint)(_OLECRF.olecrfNeedIdleTime | _OLECRF.olecrfNeedPeriodicIdleTime),
                    grfcadvf = (uint)(_OLECADVF.olecadvfModal | _OLECADVF.olecadvfRedrawOff | _OLECADVF.olecadvfWarningsOff),
                    uIdleTimeInterval = 1000
                };

                xMgr.FRegisterComponent(this, new OLECRINFO[] { crinfo }, out mComponentID);
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (mComponentID != 0)
            {
                if (GetService(typeof(SOleComponentManager)) is IOleComponentManager xMgr)
                {
                    xMgr.FRevokeComponent(mComponentID);
                }
                mComponentID = 0;
            }

            base.Dispose(disposing);
        }
    }
}
