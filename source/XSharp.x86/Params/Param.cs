using System;
using System.Collections.Generic;
using System.Linq;

namespace XSharp.x86.Params
{
    public abstract class Param {
        public delegate void ActionDelegate(object[] aValues);
        public ActionDelegate Action;
        public List<Param> Params = new List<Param>();
        public abstract bool IsMatch(object aValue);

        // Currently not used (Check refs in case used in future)
        // so may not be needed in final.
        public virtual object Transform(object aValue) {
            return aValue;
        }

        public Param Next(object aValue) {
            foreach (var xParam in Params) {
                if (xParam.IsMatch(aValue)) {
                    return xParam;
                }
            }
            throw new Exception("End of map reached.");
        }
       
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
