using System;
using System.Collections.Generic;
using System.Text;

namespace XSharp.x86 {
    public class Root {
        public delegate void Action(Params.Param[] aParams, object[] aValues);
        protected Dictionary<OpCode, List<Params.Param>> mOpCodes = new Dictionary<OpCode, List<Params.Param>>();

        public void Add(Root.Action aAction, OpCode aOpCode, params Type[] aParamTypes) {
            List<Params.Param> xParams;
            if (mOpCodes.TryGetValue(aOpCode, out xParams) == false) {
                xParams = new List<Params.Param>();
                mOpCodes[aOpCode] = xParams;
            }

            foreach (var xType in aParamTypes) {

            }
        }
    }
}
