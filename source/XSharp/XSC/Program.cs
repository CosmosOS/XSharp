using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace XSharp.CommandLine
{
    internal class Program
    {
        private static Tools.CliProcessor _Args = new Tools.CliProcessor();

        private static bool _Append = false;
        private static string _OutputPath = null;
        private static string _CPU = null;

        private static AsmGenerator _Gen = new AsmGenerator();
        // List of source files
        private static List<string> _XsFiles = new List<string>();
        private static List<Assembly> _Assemblies = new List<Assembly>();

        private static void Main(string[] aArgs)
        {
            for (int i = 0; i < aArgs.Length; i++)
            {
                Console.WriteLine(aArgs[i]);
            }
            try
            {
                _Args.Parse(aArgs);
                Run();

                Console.WriteLine("Done.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Argument parse error:\r\n{ex}");
                Environment.Exit(-2);
            }
        }

        private static void Run()
        {
            try
            {
                var xUserComments = _Args["UserComments", "UC"];
                if (xUserComments != null) _Gen.EmitUserComments = xUserComments.Check("ON", new string[] { "ON", "OFF" }).ToUpper() == "ON";
                //
                var xSourceCode = _Args["SourceCode", "SC"];
                if (xSourceCode != null) _Gen.EmitSourceCode = xSourceCode.Check("ON", new string[] { "ON", "OFF" }).ToUpper() == "ON";
                //
                var xOutput = _Args["Out", "O"];
                if (xOutput != null) _OutputPath = xOutput.Value;
                //
                _Append = _Args["Append", "A"] != null;
                if (_Append && xOutput == null) throw new Exception("Use of -Append requires use of -Out.");
                //
                var xCPU = _Args["CPU"];
                if (xCPU == null) throw new Exception("-CPU is a required parameter.");
                _CPU = xCPU.Check("", new string[] { "X86", "ARM" }, true).ToUpper();

                // Plugins
                var xPlugins = _Args.GetSwitches("PlugIn");
                foreach (var xPlugin in xPlugins)
                {
                    // TODO Load plugins
                    throw new Exception("TODO");
                }

                foreach (var xArg in _Args.Args)
                {
                    string xVal = xArg.Value;
                    if (!Path.IsPathRooted(xVal))
                    {
                        xVal = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, xVal);
                    }

                    if (Directory.Exists(xVal))
                    {
                        // If dir specified, find all .xs files
                        string xPath = Path.GetFullPath(xVal);
                        _XsFiles.AddRange(Directory.GetFiles(xPath, "*.xs"));

                    }
                    else if (File.Exists(xVal))
                    {
                        // Load .XS inputs, or Assemblies to load into compiler itself (plugins etc?)
                        string xExt = Path.GetExtension(xVal).ToUpper();
                        if (xExt == ".XS")
                        {
                            _XsFiles.Add(Path.GetFullPath(xVal));
                        }
                        else if (xExt == ".DLL")
                        {
                            _Assemblies.Add(Assembly.LoadFrom(xVal));
                        }
                        else
                        {
                            throw new Exception($"Not a valid file type: {xVal}");
                        }

                    }
                    else
                    {
                        throw new Exception($"Not a valid file or directory: {xVal}");
                    }
                }

                if (_Args["Gen2"] != null)
                {
                    RunGen2();
                }
                else
                {
                    RunGen1();
                }
            }
            catch (Exception ex)
            {
                if (_Args["WaitOnError"] != null)
                {
                    Console.WriteLine();
                    Console.WriteLine("Waiting on error. Press Enter to exit.");
                    Console.WriteLine($"Exception: {ex}");
                    Console.ReadLine();
                }
                Environment.Exit(-1);
            }
        }

        #region Gen1
        private static void RunGen1()
        {
            if (_CPU != "X86") throw new Exception("Gen1 only supports X86");
            try
            {
                // Generate output
                foreach (var xFile in _XsFiles)
                {
                    var xReader = File.OpenText(xFile);
                    if (_Append)
                    {
                        _Gen.GenerateToFile(xFile, xReader, File.AppendText(_OutputPath));
                    }
                    else if (_XsFiles.Count == 1 && _OutputPath != null)
                    {
                        _Gen.GenerateToFile(xFile, File.OpenText(xFile), File.CreateText(_OutputPath));
                    }
                    else
                    {
                        Console.WriteLine(xFile);
                        _Gen.GenerateToFiles(xFile);
                    }
                }

                // Generate output from embedded resources
                foreach (var xAssembly in _Assemblies)
                {
                    TextWriter xWriter = null;
                    if (!_Append)
                    {
                        var xDestination = Path.ChangeExtension(xAssembly.Location, "asm");
                        xWriter = new StreamWriter(File.Create(xDestination));
                    }

                    var xResources = xAssembly.GetManifestResourceNames().Where(r => r.EndsWith(".xs", StringComparison.OrdinalIgnoreCase));
                    foreach (var xResource in xResources)
                    {
                        var xStream = xAssembly.GetManifestResourceStream(xResource);
                        _Gen.GenerateToFile(xResource, new StreamReader(xStream), xWriter);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }
        #endregion

        private static void RunGen2()
        {
            if (_CPU != "X86") throw new Exception("ARM is in progress and not supported yet.");
            foreach (var xFile in _XsFiles)
            {
                using (var xIn = File.OpenText(xFile))
                {
                    if (!_Append && _OutputPath is null)
                        _OutputPath = Path.ChangeExtension(xFile, ".asm");
                    using (var xOut = File.CreateText(_OutputPath))
                    {
                        Console.WriteLine($"Processing file: {xFile}");

                        Compiler xCompiler;
                        try
                        {
                            xCompiler = new Compiler(xOut);
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex);
                            throw;
                        }

                        var xOrigBgColor = Console.BackgroundColor;
                        try
                        {
                            xCompiler.Emit(xIn);

                            Console.BackgroundColor = ConsoleColor.Green;
                            Console.WriteLine("File processed");
                            Console.BackgroundColor = xOrigBgColor;
                        }
                        catch (Exception ex)
                        {
                            Console.BackgroundColor = ConsoleColor.Red;
                            Console.WriteLine(ex);
                            Console.BackgroundColor = xOrigBgColor;
                            throw;
                        }
                    }
                }
            }
        }
    }
}
