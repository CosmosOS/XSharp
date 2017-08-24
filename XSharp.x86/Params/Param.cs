using System;
using System.Collections.Generic;
using System.Text;

namespace XSharp.x86.Params {
    public abstract class Param {
        public delegate void ActionDelegate(Params.Param[] aParams, object[] aValues);
        public ActionDelegate Action;
        public List<Param> Params = new List<Param>();
        public abstract bool IsMatch(object aValue);
    }
}
