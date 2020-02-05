namespace SDLCompiler.Domain.Models.InternalTypes
{
    public class DoubleType : Type
    {
        public override bool IsInternalType => true;
        public override bool IsArrayType => false;
        public override bool IsModelType => false;

        public DoubleType()
            : base("double")
        {
        }
    }
}
