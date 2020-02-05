using SDLCompiler.Domain.Models;
using System.Collections.Generic;

namespace SDLCompiler.Parser
{
    public class CompilationResult
    {
        public IReadOnlyList<Model> Models { get; }
        public IReadOnlyList<Service> Services { get; }

        public CompilationResult(IReadOnlyList<Model> models, IReadOnlyList<Service> services)
        {
            Models = models;
            Services = services;
        }
    }
}
