using System.Collections.Generic;

namespace SDLCompiler.Domain.Models
{
    public class Model : Type
    {
        private List<ModelMember> _members = new List<ModelMember>();

        public IReadOnlyList<ModelMember> Members => _members.AsReadOnly();
        public override bool IsInternalType => false;
        public override bool IsArrayType => false;
        public override bool IsModelType => true;

        public Model(string name)
            : base(name)
        {
        }

        public void AddMember(ModelMember member) => _members.Add(member);
    }
}
