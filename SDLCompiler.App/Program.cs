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
            // This code is for testing purposes only at the moment.
            // In the future here will be a code for the command-line compiler tool.


            string sample = @"
service CalcService
{
	int Sum(int[] numbers);
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

            foreach (var service in compilationResult.Services)
            {
                targetCodeGenerator.WriteInterfaceCode(service, Console.Out);
                targetCodeGenerator.WriteEmptyServiceCode(service, Console.Out);
                targetCodeGenerator.WriteClientServiceCode(service, Console.Out);
                targetCodeGenerator.WriteServerServiceCode(service, Console.Out);
            }
        }
    }
}
