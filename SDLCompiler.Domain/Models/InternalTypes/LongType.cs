namespace SDLCompiler.Domain.Models.InternalTypes
{
    public class LongType : Type
    {
        public override bool IsInternalType => true;
        public override bool IsArrayType => false;
        public override bool IsModelType => false;

        public LongType()
            : base("long")
        {
        }
    }
}
