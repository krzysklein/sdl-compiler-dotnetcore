using Antlr4.Runtime;
using System;
using System.IO;

namespace SDLCompiler.Parser
{
    public partial class Compiler
    {
        public CompilationResult Compile(TextReader reader)
        {
            var parser = BuildParser(reader);
            var file = parser.file();
            var visitor = new Visitor();
            visitor.Visit(file);
            var result = new CompilationResult(visitor.Models, visitor.Services);
            return result;
        }

        private SDLParser BuildParser(TextReader reader)
        {
            var stream = new AntlrInputStream(reader);
            var lexer = new SDLLexer(stream);
            var tokenStream = new CommonTokenStream(lexer);
            var parser = new SDLParser(tokenStream);
            return parser;
        }
    }
}
