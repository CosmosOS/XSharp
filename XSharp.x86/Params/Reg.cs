using System;
using System.Collections.Generic;
using System.Text;

namespace XSharp.x86.Params {
    public class Reg : List {
        public static readonly string[] Names;

        static Reg() {
            var xNames = new List<string>();
            xNames.AddRange(RegXX.Names);
            Names = xNames.ToArray();
        }

        public Reg() : base(Names) { }
        public Reg(string[] aNames) : base(aNames) { }
    }
}
