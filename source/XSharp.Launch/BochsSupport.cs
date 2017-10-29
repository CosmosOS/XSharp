using System;
using System.IO;
using System.Linq;

using Microsoft.Win32;

namespace XSharp.Launch
{
    /// <summary>
    /// An helper class that is used from both Cosmos.VS.ProjectSystem and Cosmos.VS.DebugEngine for
    /// Bochs emulator support.
    /// </summary>
    internal static class BochsSupport
    {
        private static string sBochsDirectory;
        public static string BochsDirectory
        {
            get
            {
                if (sBochsDirectory == null)
                {
                    sBochsDirectory = GetBochsDirectoryFromRegistry() ?? throw new Exception("Bochs not found!");
                }
                return sBochsDirectory;
            }
        }

        public static bool TryExtractBochsDebugSymbols(string xInputFile, string xOutputFile)
        {
            try
            {
                int i = 0;
                using (var reader = new StreamReader(File.Open(xInputFile, FileMode.Open)))
                {
                    using (var writer = new StreamWriter(File.Open(xOutputFile, FileMode.OpenOrCreate)))
                    {
                        bool startSymbolTable = false;
                        while (!reader.EndOfStream)
                        {
                            string line = reader.ReadLine();
                            if (startSymbolTable)
                            {
                                string[] items = line.Split(new[] { " " }, StringSplitOptions.RemoveEmptyEntries);
                                if (items.Length > 1)
                                {
                                    writer.WriteLine($"{items.First()} {items.Last()}");
                                    i++;
                                }
                            }
                            else if (line.Trim().ToUpper().Contains("SYMBOL TABLE"))
                            {
                                startSymbolTable = true;
                            }
                        }
                    }
                }

                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Retrieve installation path for Bochs and initialize the <see cref="BochsExe"/> property.
        /// Search is performed using the registry and rely on the shell command defined for the
        /// BochsConfigFile.
        /// </summary>
        private static string GetBochsDirectoryFromRegistry()
        {
            using (var runCommandRegistryKey = Registry.ClassesRoot.OpenSubKey(@"BochsConfigFile\shell\Run\command", false))
            {
                if (runCommandRegistryKey == null)
                {
                    return null;
                }

                string commandLine = (string)runCommandRegistryKey.GetValue(null, null);

                if (commandLine != null)
                {
                    commandLine = commandLine.Trim();
                }

                if (string.IsNullOrWhiteSpace(commandLine))
                {
                    return null;
                }

                // Now perform some parsing on command line to discover full exe path.
                string candidateFilePath;
                int commandLineLength = commandLine.Length;

                if ('"' == commandLine[0])
                {
                    // Seek for a non escaped double quote.
                    int lastDoubleQuoteIndex = 1;
                    for (; lastDoubleQuoteIndex < commandLineLength; lastDoubleQuoteIndex++)
                    {
                        if ('"' != commandLine[lastDoubleQuoteIndex]) { continue; }
                        if ('\\' != commandLine[lastDoubleQuoteIndex - 1]) { break; }
                    }

                    if (lastDoubleQuoteIndex >= commandLineLength)
                    {
                        return null;
                    }

                    candidateFilePath = commandLine.Substring(1, lastDoubleQuoteIndex - 1);
                }
                else
                {
                    // Seek for first separator character.
                    int firstSeparatorIndex = 0;
                    for (; firstSeparatorIndex < commandLineLength; firstSeparatorIndex++)
                    {
                        if (char.IsSeparator(commandLine[firstSeparatorIndex])) { break; }
                    }

                    var xSeparators = commandLine.Where(c => char.IsSeparator(c));

                    if (!xSeparators.Any())
                    {
                        return null;
                    }

                    if (firstSeparatorIndex >= commandLineLength)
                    {
                        return null;
                    }

                    candidateFilePath = commandLine.Substring(0, firstSeparatorIndex);
                }

                if (!File.Exists(candidateFilePath))
                {
                    return null;
                }

                return Path.GetDirectoryName(candidateFilePath);
            }
        }
    }
}
