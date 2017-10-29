using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;

namespace XSharp.Build.Tasks
{
    public class XsAssemble : ToolTask
    {
        enum AssemblerEnum
        {
            NASM
        }

        enum OutputFormatEnum
        {
            Bin,
            COFF,
            ELF32,
            ELF64,
            Win32,
            Win64
        }

        #region Task Parameters

        [Required]
        public string ToolsPath { get; set; }

        [Required]
        public string InputFile { get; set; }

        [Required]
        public string OutputFile { get; set; }

        [Required]
        public string Assembler
        {
            get
            {
                return mAssembler.ToString();
            }
            set
            {
#if NETCOREAPP2_0
                mAssembler = Enum.Parse<AssemblerEnum>(value, true);
#elif NET462
                mAssembler = (AssemblerEnum)Enum.Parse(typeof(AssemblerEnum), value, true);
#endif
            }
        }

        [Required]
        public string OutputFormat
        {
            get
            {
                return mOutputFormat.ToString();
            }
            set
            {
#if NETCOREAPP2_0
                mOutputFormat = Enum.Parse<OutputFormatEnum>(value, true);
#elif NET462
                mOutputFormat = (OutputFormatEnum)Enum.Parse(typeof(OutputFormatEnum), value, true);
#endif
            }
        }

#endregion

        private AssemblerEnum mAssembler;
        private OutputFormatEnum mOutputFormat;

        protected override string ToolName => Assemblers[mAssembler];
        protected override MessageImportance StandardErrorLoggingImportance => MessageImportance.High;

        private static Dictionary<AssemblerEnum, string> Assemblers = new Dictionary<AssemblerEnum, string>()
        {
            { AssemblerEnum.NASM, "nasm.exe" }
        };

        protected override bool ValidateParameters()
        {
            if (String.IsNullOrWhiteSpace(InputFile))
            {
                Log.LogError("No input file specified!");
                return false;
            }
            else if (!File.Exists(InputFile))
            {
                Log.LogError($"Input file '{InputFile}' doesn't exist!");
                return false;
            }

            if (String.IsNullOrWhiteSpace(OutputFile))
            {
                Log.LogError($"No output file specified!");
                return false;
            }

            return true;
        }

        protected override string GenerateFullPathToTool()
        {
            if (String.IsNullOrWhiteSpace(ToolExe))
            {
                return null;
            }

            if (String.IsNullOrWhiteSpace(ToolPath))
            {
                if (!String.IsNullOrWhiteSpace(ToolsPath))
                {
                    return Path.Combine(ToolsPath, Assembler, ToolExe);
                }

                return Path.Combine(Directory.GetCurrentDirectory(), ToolExe);
            }

            return Path.Combine(Path.GetFullPath(ToolPath), ToolExe);
        }

        protected override string GenerateCommandLineCommands()
        {
            var xBuilder = new CommandLineBuilder();

            switch (mAssembler)
            {
                case AssemblerEnum.NASM:
                    xBuilder.AppendSwitch("-g");

                    xBuilder.AppendSwitch("-f");
                    xBuilder.AppendSwitch(OutputFormat);

                    xBuilder.AppendSwitch("-o ");
                    xBuilder.AppendSwitch(OutputFile);

                    if (mOutputFormat == OutputFormatEnum.ELF32 || mOutputFormat == OutputFormatEnum.ELF64)
                    {
                        xBuilder.AppendSwitch("-dELF_COMPILATION");
                    }

                    xBuilder.AppendSwitch("-O0");

                    xBuilder.AppendFileNameIfNotNull(InputFile);
                    break;
                default:
                    throw new NotImplementedException($"Assembler '{mAssembler}' not implemented!");
            }

            return xBuilder.ToString();
        }
    }
}
