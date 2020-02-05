using SDLCompiler.Domain;
using SDLCompiler.Parser;
using SDLCompiler.Targets.CSharp;
using System;
using System.IO;

namespace SDLCompiler.App
{
    class Program
    {
        static void Main(string[] args)
        {
            string sample = @"
// Simple Hello Service

model HelloRequest
{
	string[] Names;
}

model HelloResult
{
	string Output;
}

service HelloService
{
	HelloResult SayHello(HelloRequest request);
}
";

            var compiler = new Compiler();
            var reader = new StringReader(sample);
            var compilationResult = compiler.Compile(reader);

            ITargetCodeGenerator targetCodeGenerator = new CSharpTargetCodeGenerator();

            foreach (var model in compilationResult.Models)
            {
                targetCodeGenerator.WriteModelCode(model, Console.Out);

            }

#if DEBUG
            Console.WriteLine("End.");
            Console.ReadKey();
#endif
        }
    }
}
