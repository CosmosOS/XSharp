using System;

namespace XSharp.Launch.Hosts.IntelEdison
{
    public class IntelEdisonHost : IHost
    {
        public event EventHandler ShutDown;

        public IntelEdisonHost()
        {
        }

        public void Start()
        {
        }

        public void Kill()
        {
            ShutDown?.Invoke(this, EventArgs.Empty);
        }
    }
}
