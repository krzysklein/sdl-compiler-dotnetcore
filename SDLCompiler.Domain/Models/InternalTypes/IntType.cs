namespace SDLCompiler.Domain.Models.InternalTypes
{
    public class IntType : Type
    {
        public override bool IsInternalType => true;
        public override bool IsArrayType => false;
        public override bool IsModelType => false;

        public IntType()
            : base("int")
        {
        }
    }
}
