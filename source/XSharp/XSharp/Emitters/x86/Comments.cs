using Spruce.Attribs;
using Spruce.Tokens;
using XSharp.Tokens;

namespace XSharp.x86.Emitters
{
    /// <summary>
    /// Class that processes comments and literals for X#.
    /// </summary>
    /// <seealso cref="XSharp.x86.Emitters.Emitters" />
    public class Comments : Emitters
    {
        public Comments(Compiler aCompiler, x86.Assemblers.Assembler aAsm) : base(aCompiler, aAsm)
        {
        }

        /// <summary>
        /// Literal NASM output. The text will be directly printed out by
        /// the compiler and will not be processed
        /// </summary>
        /// <example>
        /// //! Value <br/>
        /// Output: Value
        /// </example>
        [Emitter(typeof(OpLiteral), typeof(All))]
        protected void Literal(string aOp, string aText)
        {
            Compiler.WriteLine(aText);
        }

        /// <summary>
        /// Comments output. The value is not processed and printed out
        /// with a preceeding semicolon.
        /// This is specifically for empty comments
        /// </summary>
        /// <example>
        /// // Comment text <br/>
        /// Output: ; Comment text
        /// </example>
        /// <seealso cref="Comment"/>
        [Emitter(typeof(OpComment))] // //
        protected void CommentEmpty(string aOp)
        {
        }

        /// <summary>
        /// Comments output. The value is not processed and printed out
        /// with a preceeding semicolon.
        /// </summary>
        /// <example>
        /// // Comment text <br/>
        /// Output: ; Comment text
        /// </example>
        [Emitter(typeof(OpComment), typeof(All))]
        protected void Comment(string aOp, string aText)
        {
            if (Compiler.EmitUserComments)
            {
                Compiler.WriteLine("; " + aText);
            }
        }
    }
}
