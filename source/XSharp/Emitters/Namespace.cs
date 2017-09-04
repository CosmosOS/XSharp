using Spruce.Attribs;
using Spruce.Tokens;
using XSharp.Tokens;

namespace XSharp.Emitters
{
    /// <summary>
    /// Namespaces for X# files. All files must start with a namespace.
    /// </summary>
    /// <seealso cref="XSharp.Emitters.Emitters" />
    public class Namespace : Emitters
    {
        public Namespace(Compiler aCompiler, x86.Assemblers.Assembler aAsm) : base(aCompiler, aAsm)
        {
        }

        /// <summary>
        /// Definition of a namespace. Does not generate any code.
        /// </summary>
        [Emitter(typeof(NamespaceKeyword), typeof(AlphaNum))] // namespace name
        protected void NamespaceDefinition(string aNamespaceKeyword, string aText)
        {
            Compiler.CurrentNamespace = aText;
        }
    }
}
