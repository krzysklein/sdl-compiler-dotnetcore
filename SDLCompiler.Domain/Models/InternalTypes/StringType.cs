namespace SDLCompiler.Domain.Models.InternalTypes
{
    public class StringType : Type
    {
        public override bool IsInternalType => true;
        public override bool IsArrayType => false;
        public override bool IsModelType => false;

        public StringType()
            : base("string")
        {
        }
    }
}
