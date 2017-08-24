using System;
using System.Collections.Generic;
using System.Text;

namespace XSharp.x86.Params {
    public class RegXX : Reg {
        public static readonly string[] Names;

        static RegXX() {
            var xNames = new List<string>();
            xNames.AddRange(Reg32.Names);
            xNames.AddRange(Reg16.Names);
            xNames.AddRange(Reg08.Names);
            Names = xNames.ToArray();
        }
    }
}
