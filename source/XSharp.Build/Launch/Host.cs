using System;
using System.Collections.Generic;

namespace XSharp.Build.Launch
{
    public abstract class Host
    {
        protected bool mUseGDB;

        public EventHandler OnShutDown;

        public Host(bool aUseGDB)
        {
            mUseGDB = aUseGDB;
        }

        public abstract void Start();
        public abstract void Stop();
    }
}
