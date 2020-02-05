using System.Collections.Generic;

namespace SDLCompiler.Domain.Models
{
    public class ServiceMethod
    {
        private List<ServiceMethodParameter> _methodParameters = new List<ServiceMethodParameter>();

        public Type ReturnType { get; }
        public string Name { get; }
        public IReadOnlyList<ServiceMethodParameter> MethodParameters => _methodParameters.AsReadOnly();

        public ServiceMethod(Type returnType, string name)
        {
            ReturnType = returnType;
            Name = name;
        }

        public void AddMethodParameter(ServiceMethodParameter methodParameter) => _methodParameters.Add(methodParameter);

    }
}
