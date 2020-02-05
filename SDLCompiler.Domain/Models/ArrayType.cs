namespace SDLCompiler.Domain.Models
{
    public class ArrayType : Type
    {
        public Type BaseType { get; }
        public int NumArrayDimensions { get; }
        public override bool IsInternalType => BaseType.IsInternalType;
        public override bool IsArrayType => true;
        public override bool IsModelType => false;

        public ArrayType(Type baseType, string name, int numArrayDimensions) 
            : base(name)
        {
            BaseType = baseType;
            NumArrayDimensions = numArrayDimensions;
        }

    }
}
