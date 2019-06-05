using System;
using System.IO;

namespace XSharp.Tools
{
    public class Clogger : IDisposable {
        public enum FileType {
            Overwrite,
            Append,
            Timestamped
        }

        public TextWriter Out { get; protected set; }
        public bool CopyToStdOut { get; set; }

        public Clogger(TextWriter aOut) {
            Out = aOut;
        }
        public Clogger(string aPath, FileType aType = FileType.Overwrite) {
            if (aType == FileType.Overwrite) {
                Out = File.CreateText(aPath);
            } else if (aType == FileType.Append) {
                Out = File.AppendText(aPath);
                NewSection("New Log @ " + TimeStamp());
            } else if (aType == FileType.Timestamped) {
                string xExt = Path.GetExtension(aPath);
                aPath = Path.GetFileNameWithoutExtension(aPath) + "-" + TimeStamp() + xExt;
                Out = File.CreateText(aPath);
            } else {
                throw new Exception("Unknown FileType in Clogger(): " + aType);
            }
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                Out.Dispose();
                Out = null;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public static string TimeStamp() {
            return DateTime.Now.ToString("yyyyMMdd'-'HHmmss");
        }

        public virtual void Write(string aText) {
            if (CopyToStdOut) {
                Console.Write(aText);
            }
            Out.Write(aText);
        }
        public virtual void WriteLine(string aText) {
            if (CopyToStdOut) {
                Console.WriteLine(aText);
            }
            Out.WriteLine(aText);
        }
        public virtual void NewSection(string aText) {
            if (CopyToStdOut) {
                Console.WriteLine("======================================");
                Console.WriteLine(aText);
            }
            Out.WriteLine(aText);
        }
        public virtual void WriteBlankLine() {
            if (CopyToStdOut) {
                Console.WriteLine();
            }
            Out.WriteLine();
        }
        public virtual void WriteException(Exception aEx) {
            WriteLine("!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!");
            WriteLine(aEx.Message);
            var xInner = aEx.InnerException;
            while (xInner != null) {
                WriteLine("!!!!");
                WriteLine(xInner.Message);
                xInner = xInner.InnerException;
            }
            WriteLine("!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!");
        }

#pragma warning disable CA2225 // Operator overloads have named alternates
        public static Clogger operator *(Clogger aThis, string aValue)
        {
            aThis.WriteLine(aValue);
            return aThis;
        }
        public static Clogger operator +(Clogger aThis, string aValue) {
            aThis.Write(aValue);
            return aThis;
        }
        public static Clogger operator /(Clogger aThis, string aValue) {
            aThis.NewSection(aValue);
            return aThis;
        }
        public static Clogger operator ++(Clogger aThis) {
            aThis.WriteBlankLine();
            return aThis;
        }
#pragma warning restore CA2225 // Operator overloads have named alternates
    }
}
