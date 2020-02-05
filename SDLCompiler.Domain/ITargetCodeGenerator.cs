using SDLCompiler.Domain.Models;
using System.IO;

namespace SDLCompiler.Domain
{
    public interface ITargetCodeGenerator
    {
        void WriteModelCode(Model model, TextWriter writer);
        void WriteClientServiceCode(Service service, TextWriter writer);
        void WriteServerServiceCode(Service service, TextWriter writer);
    }
}
