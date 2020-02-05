namespace SDLCompiler.Domain.Models
{
    public class ServiceMethodParameter
    {
        public Type Type { get; }
        public string Name { get; }

        public ServiceMethodParameter(Type type, string name)
        {
            Type = type;
            Name = name;
        }
    }
}
