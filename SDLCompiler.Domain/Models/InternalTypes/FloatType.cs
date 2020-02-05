namespace SDLCompiler.Domain.Models.InternalTypes
{
    public class FloatType : Type
    {
        public override bool IsInternalType => true;
        public override bool IsArrayType => false;
        public override bool IsModelType => false;

        public FloatType()
            : base("float")
        {
        }
    }
}
