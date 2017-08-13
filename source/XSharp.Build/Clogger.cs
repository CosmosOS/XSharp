using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace XSharp.Build {
    public class Clogger {
        public enum FileType {
            Overwrite,
            Append,
            Timestamped
        }

        public readonly TextWriter Out;

        public Clogger(TextWriter aOut) {
            Out = aOut;
        }
        public Clogger(string aPath, FileType aType = FileType.Overwrite) {
            if (aType == FileType.Overwrite) {
                Out = File.CreateText(aPath);
            } else if (aType == FileType.Append) {
                Out = File.AppendText(aPath);
            } else if (aType == FileType.Timestamped) {
                string xExt = Path.GetExtension(aPath);
                aPath = Path.GetFileNameWithoutExtension(aPath) + "-" + TimeStamp() + xExt;
                Out = File.CreateText(aPath);
            } else {
                throw new Exception("Unknown FileType in Clogger(): " + aType);
            }
        }

        public string TimeStamp() {
            return DateTime.Now.ToString("yyyyMMdd'-'HHmmss");
        }
    }
}
