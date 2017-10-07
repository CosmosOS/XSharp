using System;
using System.Collections.Generic;
using System.CommandLine;
using System.IO;
using System.Linq;
using System.Reflection;

namespace XSharp.XSC
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                new CommandLineApplication().Execute(args);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception occurred:");
                Console.WriteLine(ex);
                Console.WriteLine("Press any key to exit...");
                Console.ReadKey();
            }
        }
    }

    class CommandLineApplication
    {
        private bool mHelp;

        private bool mGen2;
        private bool mAppend;
        private string mOutputPath;
        private IReadOnlyList<string> mInputs;
        private IReadOnlyList<string> mPlugins;

        public void Execute(string[] args)
        {
            Console.WriteLine();

            var xArgSyntax = ArgumentSyntax.Parse(args, (s) =>
            {
                s.ApplicationName = "xsc";
                s.DefineOption("h|help", ref mHelp, "Display help");

                s.DefineOption("gen2", ref mGen2, "Defines if the Gen2 of the compiler should be used.");
                s.DefineOption("a|append", ref mAppend, "Defines if the compilation result should be written to a single file");
                s.DefineOption("o|output", ref mOutputPath, "The output path");
                s.DefineOptionList("p|plugins", ref mPlugins, "The plugins to load in the compiler");
                s.DefineParameterList("source", ref mInputs, "The files to compile");
            });

            if (mHelp)
            {
                Console.WriteLine(xArgSyntax.GetHelpText());
                return;
            }

            if (mAppend && mOutputPath == null)
            {
                throw new Exception("Use of --append requires use of --output.");
            }

            if (mPlugins != null)
            {
                foreach (var xPlugin in mPlugins)
                {
                    // TODO Load plugins
                }
            }

            // List of source files
            var xFiles = new List<string>();
            var xAssemblies = new List<Assembly>();
            foreach (var xInput in mInputs)
            {
                if (Directory.Exists(xInput))
                {
                    // If dir specified, find all .xs files
                    string xPath = Path.GetFullPath(xInput);
                    xFiles.AddRange(Directory.GetFiles(xPath, "*.xs"));
                }
                else if (File.Exists(xInput))
                {
                    string xExt = Path.GetExtension(xInput).ToLower();
                    if (xExt == ".xs")
                    {
                        xFiles.Add(Path.GetFullPath(xInput));
                    }
                    else if (xExt == ".dll")
                    {
                        xAssemblies.Add(Assembly.ReflectionOnlyLoadFrom(xInput));
                    }
                    else
                    {
                        throw new Exception("Not a valid file type: " + xInput);
                    }
                }
                else
                {
                    throw new Exception("Not a valid file or directory: " + xInput);
                }
            }

            if (mGen2)
            {
                foreach (var xFile in xFiles)
                {
                    using (var xIn = File.OpenText(xFile))
                    {
                        if (!mAppend)
                        {
                            mOutputPath = Path.ChangeExtension(xFile, ".asm");
                        }

                        using (var xOut = File.CreateText(mOutputPath))
                        {
                            Console.WriteLine("Processing file: " + xFile);
                            Compiler xCompiler;

                            try
                            {
                                xCompiler = new Compiler(xOut);
                            }
                            catch (Exception e)
                            {
                                Console.WriteLine(e);
                                throw;
                            }
                            try
                            {
                                xCompiler.Emit(xIn);
                                var temp = Console.BackgroundColor;
                                Console.BackgroundColor = ConsoleColor.Green;
                                Console.WriteLine("File processed");
                                Console.BackgroundColor = temp;
                            }
                            catch (Exception e)
                            {
                                var temp = Console.BackgroundColor;
                                Console.BackgroundColor = ConsoleColor.Red;
                                Console.WriteLine(e);
                                Console.BackgroundColor = temp;
                                throw;
                            }
                        }
                    }
                }
            }
            else
            {
                try
                {
                    var xGen = new AsmGenerator();

                    // Generate output
                    foreach (var xFile in xFiles)
                    {
                        var xReader = File.OpenText(xFile);
                        if (mAppend)
                        {
                            xGen.Generate(xReader, File.AppendText(mOutputPath));
                        }
                        else if (xFiles.Count == 1 && mOutputPath != null)
                        {
                            xGen.Generate(File.OpenText(xFile), File.CreateText(mOutputPath));
                        }
                        else
                        {
                            Console.WriteLine(xFile);
                            xGen.GenerateToFiles(xFile);
                        }
                    }

                    // Generate output from embedded resources
                    foreach (var xAssembly in xAssemblies)
                    {
                        TextWriter xWriter = null;
                        if (!mAppend)
                        {
                            var xDestination = Path.ChangeExtension(xAssembly.Location, "asm");
                            xWriter = new StreamWriter(File.Create(xDestination));
                        }

                        foreach (var xResource in xAssembly.GetManifestResourceNames()
                          .Where(r => r.EndsWith(".xs", StringComparison.OrdinalIgnoreCase)))
                        {
                            var xStream = xAssembly.GetManifestResourceStream(xResource);
                            xGen.GenerateToFile(xResource, new StreamReader(xStream), xWriter);
                        }
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }
            }

            // Finalize
            Console.WriteLine("Done.");
        }
    }
}
