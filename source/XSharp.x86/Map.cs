using System;
using System.Collections.Generic;

namespace XSharp.x86
{
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

        // Use Params to transform values and call Action with transformed values.
        public void Execute(OpCode aOp, params object[] aParams) {
            Params.Param xParam;
            if (mOpCodes.TryGetValue(aOp, out xParam) == false) {
                throw new Exception("No OpCode found in map for : " + aOp);
            }

            for (int i = 0; i < aParams.Length; i++) {
                xParam = xParam.Next(aParams[i]);
                aParams[i] = xParam.Transform(aParams[i]);
            }
            if (xParam.Params.Count > 0) {
                throw new Exception("End param has subparams.");
            }

            xParam.Action(aParams);
        }
    }
}
