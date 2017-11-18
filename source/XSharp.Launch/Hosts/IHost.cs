using System;

namespace XSharp.Launch.Hosts
{
    public interface IHost
    {
        event EventHandler ShutDown;

        void Start();
        void Stop();
    }
}
