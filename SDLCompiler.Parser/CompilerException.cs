using System;

namespace SDLCompiler.Parser
{
    public class CompilerException : Exception
    {
        public CompilerException(string message)
            : base(message)
        {

        }
    }
}
