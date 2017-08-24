using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace XSharp.x86.Params {
    public abstract class Param {
        public delegate void ActionDelegate(Params.Param[] aParams, object[] aValues);
        public ActionDelegate Action;
        public List<Param> Params = new List<Param>();
        public abstract bool IsMatch(object aValue);

        public Param Add(Type aParamType) {
            var xParam = Params.SingleOrDefault(q => q.GetType() == aParamType);

            if (xParam == null) {
                if (Action != null) {
                    throw new Exception("Cannot add subparams to a param which has an action.");
                }
                xParam = (Param)Activator.CreateInstance(aParamType);
                Params.Add(xParam);
            }

            return xParam;
        }
    }
}
