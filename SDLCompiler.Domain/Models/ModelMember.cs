namespace SDLCompiler.Domain.Models
{
    public class ModelMember
    {
        public Type Type { get; }
        public string Name { get; }

        public ModelMember(Type type, string name)
        {
            Type = type;
            Name = name;
        }
    }
}
