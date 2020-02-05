namespace SDLCompiler.Domain.Models.InternalTypes
{
    public class CharType : Type
    {
        public override bool IsInternalType => true;
        public override bool IsArrayType => false;
        public override bool IsModelType => false;

        public CharType()
            : base("char")
        {
        }
    }
}
