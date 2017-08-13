using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace XSharp.Build {
    public class Clogger : IDisposable {
        public enum FileType {
            Overwrite,
            Append,
            Timestamped
        }

        public TextWriter Out { get; protected set; }

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

        public void Dispose() {
            Out.Dispose();
            Out = null;
        }

        public string TimeStamp() {
            return DateTime.Now.ToString("yyyyMMdd'-'HHmmss");
        }

        public static Clogger operator *(Clogger aThis, string aValue) {
            aThis.Out.WriteLine(aValue);
            return aThis;
        }
        public static Clogger operator +(Clogger aThis, string aValue) {
            aThis.Out.Write(aValue);
            return aThis;
        }
        public static Clogger operator ++(Clogger aThis) {
            aThis.Out.WriteLine();
            return aThis;
        }
    }
}
