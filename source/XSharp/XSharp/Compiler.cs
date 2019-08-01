using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using XSharp.x86.Assemblers;

namespace XSharp
{
    public class Compiler
    {
        protected Spruce.Tokens.Root mTokenMap;
        public readonly TextWriter Out;
        protected readonly NASM mNASM;
        protected string Indent = "";
        public int LineNo { get; private set; }
        public bool EmitUserComments = true;
        public bool EmitSourceCode = true;

        private string _currentNamespace;

        /// <summary>
        /// Gets the current namespace.
        /// </summary>
        public string CurrentNamespace
        {
            get
            {
                if (string.IsNullOrEmpty(_currentNamespace))
                    throw new Exception("Namespace not available. Make sure that the file begins with a namespace");
                return _currentNamespace;
            }
            set => _currentNamespace = value;
        }

        /// <summary>
        /// Gets the current function.
        /// </summary>
        public string CurrentFunction { get; set; }

        /// <summary>The set of blocks for the currently assembled function. Each time we begin
        /// assembling a new function this blocks collection is reset to an empty state.</summary>
        public static BlockList Blocks = new BlockList();

        public class BlockList : List<Block>
        {
            protected int mCurrentLabelID = 0;

            public void Reset()
            {
                mCurrentLabelID = 0;
            }

            public Block Current()
            {
                return base[Count - 1];
            }

            public void Start(TokenList aTokens, bool aIsCollector)
            {
                var xBlock = new Block();
                mCurrentLabelID++;
                xBlock.LabelID = mCurrentLabelID;
                xBlock.StartTokens = aTokens;

                // Last because we use Current() above
                Add(xBlock);
                xBlock.ParentAssembler = Assembler.Assembler.CurrentInstance;
                new Assembler.Assembler();
            }

            public void End()
            {
                Assembler.Assembler.ClearCurrentInstance();
                RemoveAt(Count - 1);
            }
        }
        public class Block
        {
            public TokenList StartTokens;
            public int LabelID;

            public Assembler.Assembler ParentAssembler;

            public void AddContentsToParentAssembler()
            {
                ParentAssembler.Instructions.AddRange(Assembler.Assembler.CurrentInstance.Instructions);
            }
        }

        public Compiler(TextWriter aOut)
        {
            Out = aOut;
            mNASM = new NASM(aOut);

            mTokenMap = new Spruce.Tokens.Root();
            mTokenMap.AddEmitter(new Emitters.Namespace(this, mNASM));
            mTokenMap.AddEmitter(new Emitters.Comments(this, mNASM));
            mTokenMap.AddEmitter(new Emitters.Ports(this, mNASM));
            mTokenMap.AddEmitter(new Emitters.ZeroParamOps(this, mNASM)); // This should be above push/pop
            mTokenMap.AddEmitter(new Emitters.IncrementDecrement(this, mNASM)); // This should be above + operator
            mTokenMap.AddEmitter(new Emitters.PushPop(this, mNASM)); // This should be above + operator
            mTokenMap.AddEmitter(new Emitters.Assignments(this, mNASM));
            mTokenMap.AddEmitter(new Emitters.Test(this, mNASM));
            mTokenMap.AddEmitter(new Emitters.Math(this, mNASM));
            mTokenMap.AddEmitter(new Emitters.ShiftRotate(this, mNASM));
            mTokenMap.AddEmitter(new Emitters.AllEmitters(this, mNASM));
        }

        public void WriteLine(string aText = "")
        {
            Out.WriteLine(Indent + aText);
        }

        public void Emit(TextReader aIn)
        {
            try
            {
                LineNo = 1;
                // Do not trim it here. We need spaces for colorizing
                // and also to keep indentation in the output.
                string xText = aIn.ReadLine();
                while (xText != null)
                {
                    int i = xText.Length - xText.TrimStart().Length;
                    mNASM.Indent = Indent = xText.Substring(0, i);

                    if (string.IsNullOrWhiteSpace(xText))
                    {
                        WriteLine();
                    }
                    else if (xText == "//END")
                    {
                        // Temp hack, remove in future
                        break;
                    }
                    else
                    {
                        var xCodePoints = mTokenMap.Parse(xText);
                        var xLastToken = xCodePoints.Last().Token;
                        if (EmitSourceCode && (xCodePoints[0].Token is Tokens.OpComment == false))
                        {
                            WriteLine("; " + xText.Trim());
                        }
                        xLastToken.Emitter(xCodePoints);
                    }

                    xText = aIn.ReadLine();
                    LineNo++;
                }
            }
            catch (Exception e)
            {
                throw new Exception("Generation error on line " + LineNo, e);
            }
        }
    }
}
