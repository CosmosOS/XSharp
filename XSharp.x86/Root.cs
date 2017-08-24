using System;
using System.Collections.Generic;
using System.Text;

namespace XSharp.x86 {
    public class Map {
        protected Dictionary<OpCode, Params.Param> mOpCodes = new Dictionary<OpCode, Params.Param>();

        public void Add(Params.Param.ActionDelegate aAction, OpCode aOpCode, params Type[] aParamTypes) {
            // See if OpCode already mapped, if not create a slot
            Params.Param xParam;
            if (mOpCodes.TryGetValue(aOpCode, out xParam) == false) {
                xParam = new Params.Root();
                mOpCodes[aOpCode] = xParam;
            }

            foreach (var xType in aParamTypes) {
                xParam = xParam.Add(xType);
            }

            if (xParam.Params.Count > 0) {
                throw new Exception("Cannot add action to a param which has subparams.");
            }
            xParam.Action = aAction;
        }
    }
}
