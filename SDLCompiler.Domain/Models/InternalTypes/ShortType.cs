namespace SDLCompiler.Domain.Models.InternalTypes
{
    public class ShortType : Type
    {
        public override bool IsInternalType => true;
        public override bool IsArrayType => false;
        public override bool IsModelType => false;

        public ShortType()
            : base("short")
        {
        }
    }
}
