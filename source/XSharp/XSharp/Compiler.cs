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
        /// The current namespace.
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
        /// The current function.
        /// </summary>
        public string CurrentFunction { get; set; }

        /// <summary>
        /// The current function exit label.
        /// </summary>
        public string CurrentFunctionExitLabel
        {
            get
            {
                if (string.IsNullOrWhiteSpace(CurrentFunction))
                {
                    throw new Exception("Current Function not available.");
                }
                return $"{CurrentNamespace}_{CurrentFunction}_Exit";
            }
        }

        /// <summary>
        /// The type of the current function.
        /// </summary>
        public BlockType CurrentFunctionType { get; set; }

        /// <summary>
        /// The current label.
        /// </summary>
        public string CurrentLabel { get; set; }

        /// <summary>
        /// Did we find an explicit 'Exit:' label?
        /// </summary>
        public bool FunctionExitLabelFound { get; set; }

        public string GetFullName(string aName, bool isLabel = false)
        {
            if (!string.IsNullOrWhiteSpace(CurrentFunction) && isLabel)
            {
                return $"{CurrentNamespace}_{CurrentFunction}_{aName}";
            }
            return $"{CurrentNamespace}_{aName}";
        }

        /// <summary>
        /// The set of blocks for the currently assembled function.
        /// Each time we begin assembling a new function this blocks collection is reset to an empty state.
        /// </summary>
        public BlockList Blocks { get; }
        public class BlockList
        {
            private List<Block> mBlocks = new List<Block>();

            private Compiler mCompiler;

            protected int mCurrentLabelID = 0;

            public BlockList(Compiler aCompiler)
            {
                mCompiler = aCompiler;
            }

            public void Reset()
            {
                mCurrentLabelID = 0;
                mBlocks.Clear();
            }

            public void StartBlock(BlockType aType)
            {
                mCurrentLabelID++;

                var xBlock = new Block
                {
                    LabelID = mCurrentLabelID,
                    Type = aType
                };
                mBlocks.Add(xBlock);
            }

            public void EndBlock()
            {
                mCompiler.WriteLine($"{EndBlockLabel}:");
                mBlocks.RemoveAt(mBlocks.Count - 1);
            }

            public string EndBlockLabel => $"{mCompiler.CurrentNamespace}_{mCompiler.CurrentFunction}_Block{Current().LabelID}_End";

            public Block Current()
            {
                if (!mBlocks.Any())
                {
                    return null;
                }

                return mBlocks[mBlocks.Count - 1];
            }
        }
        public class Block
        {
            public BlockType Type { get; set; }
            public int LabelID { get; set; }
        }
        public enum BlockType
        {
            None,
            Function,
            If,
            Interrupt,
            Label,
            Repeat,
            While
        }

        public Compiler(TextWriter aOut)
        {
            Blocks = new BlockList(this);

            Out = aOut;
            mNASM = new NASM(aOut);

            mTokenMap = new Spruce.Tokens.Root();
            mTokenMap.AddEmitter(new x86.Emitters.Namespace(this, mNASM));
            mTokenMap.AddEmitter(new x86.Emitters.Comments(this, mNASM));
            mTokenMap.AddEmitter(new x86.Emitters.Ports(this, mNASM));
            mTokenMap.AddEmitter(new x86.Emitters.ZeroParamOps(this, mNASM)); // This should be above push/pop
            mTokenMap.AddEmitter(new x86.Emitters.IncrementDecrement(this, mNASM)); // This should be above + operator
            mTokenMap.AddEmitter(new x86.Emitters.PushPop(this, mNASM)); // This should be above + operator
            mTokenMap.AddEmitter(new x86.Emitters.Assignments(this, mNASM));
            mTokenMap.AddEmitter(new x86.Emitters.Test(this, mNASM));
            mTokenMap.AddEmitter(new x86.Emitters.Math(this, mNASM));
            mTokenMap.AddEmitter(new x86.Emitters.ShiftRotate(this, mNASM));
            mTokenMap.AddEmitter(new x86.Emitters.Branching(this, mNASM));
            mTokenMap.AddEmitter(new x86.Emitters.BitwiseEmitters(this, mNASM));
            mTokenMap.AddEmitter(new x86.Emitters.AllEmitters(this, mNASM));
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
