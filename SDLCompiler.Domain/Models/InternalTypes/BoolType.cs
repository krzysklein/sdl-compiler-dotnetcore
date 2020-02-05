﻿namespace SDLCompiler.Domain.Models.InternalTypes
{
    public class BoolType : Type
    {
        public override bool IsInternalType => true;
        public override bool IsArrayType => false;
        public override bool IsModelType => false;

        public BoolType()
            : base("bool")
        {
        }
    }
}
