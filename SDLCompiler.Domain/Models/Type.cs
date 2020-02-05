namespace SDLCompiler.Domain.Models
{
    public abstract class Type
    {
        public string Name { get; }
        public abstract bool IsInternalType { get; }
        public abstract bool IsArrayType { get; }
        public abstract bool IsModelType { get; }

        protected Type(string name)
        {
            Name = name;
        }
    }
}
