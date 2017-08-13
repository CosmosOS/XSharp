using System;
using System.Collections.Generic;
using System.Text;

namespace XSharp.Build {
    public class CliProcessor {
        public class Item {
            public string Value;
            public Switch Switch;
        }

        public class Switch {
            public string Name;
            public string Value;
        }

        public List<Item> Items = new List<Item>();
        // Do not use dictionary. Dictionary loses order and dose not allow multiples.
        public List<Switch> Switches = new List<Switch>();

        public CliProcessor(string[] aArgs, bool aAllowNone = false) {
            if (aAllowNone == false && aArgs.Length == 0) {
                throw new Exception("No arguments were specified.");
            }

            Switch xSwitch = null;
            foreach (var xArg in aArgs) {
                if (xArg.StartsWith("-")) {
                    xSwitch = new Switch();
                    var xParts = xArg.Substring(1).ToUpper().Split(':');
                    xSwitch.Name = xParts[0];
                    if (xParts.Length > 1) {
                        xSwitch.Value = xParts[1];
                    }
                    Switches.Add(xSwitch);
                } else {
                    var xItem = new Item();
                    xItem.Value = xArg;
                    xItem.Switch = xSwitch;
                    Items.Add(xItem);
                }
            }
        }
    }
}
