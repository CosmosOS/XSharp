using System;
using System.Collections.Generic;
using System.Text;

namespace XSharp.Build {
    public class CliProcessor {
        public class Item {
            public readonly int OrigIdx = -1;
            public readonly string Value;
            public readonly Item Previous;
            public readonly Item Next;

            public Item(int aIdx, string aValue, Item aPrev, Item aNext) {
                OrigIdx = aIdx;
                Value = aValue;
                Previous = aPrev;
                Next = aNext;
            }
        }

        public class Switch : Item {
            public readonly string Name;

            public Switch(int aIdx, string aName, string aValue, Item aPrev, Item aNext) : base(aIdx, aValue, aPrev, aNext) {
                Name = aName;
            }
        }

        public List<Item> All = new List<Item>();
        public List<Item> Files = new List<Item>();
        public List<Item> Others = new List<Item>();
        public List<Switch> Switches = new List<Switch>();

        public CliProcessor(string[] aArgs, bool aAllowNone = false) {
            if (aAllowNone == false && aArgs.Length == 0) {
                throw new Exception("No arguments were specified.");
            }
        }
    }
}
