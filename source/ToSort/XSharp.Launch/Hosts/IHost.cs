using System;

namespace XSharp.Launch.Hosts
{
    public interface IHost
    {
        event EventHandler ShutDown;

        void Start();
        // not Stop, because it's a keyword in Visual Basic
        void Kill();
    }
}
