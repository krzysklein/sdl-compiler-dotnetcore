using System;
using System.Collections.Generic;

namespace SDLCompiler.Domain.Models
{
    public class Service
    {
        private List<ServiceMethod> _methods = new List<ServiceMethod>();

        public string Name { get; }
        public IReadOnlyList<ServiceMethod> Methods => _methods.AsReadOnly();

        public Service(string name)
        {
            Name = name;
        }

        public void AddServiceMethod(ServiceMethod serviceMethod) => _methods.Add(serviceMethod);
    }
}
