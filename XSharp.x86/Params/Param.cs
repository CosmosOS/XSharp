using System;
using System.Collections.Generic;
using System.Text;

namespace XSharp.x86.Params {
    public abstract class Param {
        protected List<Param> mParams = new List<Param>();
        public Action<List<Param>> Emitter;
        public abstract bool IsMatch(object aValue);
    }
}
