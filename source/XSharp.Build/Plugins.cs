using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace XSharp.Build {
    public static class Plugins {
        public static List<Assembly> List = new List<Assembly>();

        public static void Load(string aPath) {
        }

        public static List<T> Get<T>() {
            return null;
        }
    }
}
