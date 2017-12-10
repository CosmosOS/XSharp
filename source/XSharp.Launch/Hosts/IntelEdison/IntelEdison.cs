using System;

namespace XSharp.Launch.Hosts.IntelEdison
{
    public class IntelEdison : IHost
    {
        public event EventHandler ShutDown;

        public IntelEdison()
        {
        }

        public void Start()
        {
        }

        public void Stop()
        {
            ShutDown?.Invoke(this, EventArgs.Empty);
        }
    }
}
