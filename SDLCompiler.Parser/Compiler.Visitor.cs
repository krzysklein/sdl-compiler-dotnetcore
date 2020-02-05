using System.Collections.Generic;
using System.Linq;
using Antlr4.Runtime.Misc;
using SDLCompiler.Domain.Models;
using SDLCompiler.Domain.Models.InternalTypes;

namespace SDLCompiler.Parser
{
    public partial class Compiler
    {
        private class Visitor : SDLBaseVisitor<object>
        {
            private readonly List<Model> _models = new List<Model>();
            private readonly List<Service> _services = new List<Service>();

            public IReadOnlyList<Model> Models => _models.AsReadOnly();
            public IReadOnlyList<Service> Services => _services.AsReadOnly();

            public override object VisitModelDeclaration([NotNull] SDLParser.ModelDeclarationContext context)
            {
                var modelName = context.Identifier().GetText();
                var model = new Model(modelName);
                _models.Add(model);

                foreach (var modelMemberContext in context.modelMember())
                {
                    var modelMember = VisitModelMember(modelMemberContext) as ModelMember 
                        ?? throw new CompilerException("Fatal error: null model member!");
                    model.AddMember(modelMember);
                }

                return model;
            }

            public override object VisitModelMember([NotNull] SDLParser.ModelMemberContext context)
            {
                var type = VisitType(context.type()) as Type;
                var name = context.Identifier().GetText();
                var modelMember = new ModelMember(type, name);
                return modelMember;
            }

            public override object VisitServiceDeclaration([NotNull] SDLParser.ServiceDeclarationContext context)
            {
                var serviceName = context.Identifier().GetText();
                var service = new Service(serviceName);
                _services.Add(service);

                foreach (var serviceMethodContext in context.serviceMethod())
                {
                    var serviceMethod = VisitServiceMethod(serviceMethodContext) as ServiceMethod
                        ?? throw new CompilerException("Fatal error: null service method!");
                    service.AddServiceMethod(serviceMethod);
                }

                return service;
            }

            public override object VisitServiceMethod([NotNull] SDLParser.ServiceMethodContext context)
            {
                var returnType = VisitTypeOrVoid(context.typeOrVoid()) as Type;
                var name = context.Identifier().GetText();
                var serviceMethod = new ServiceMethod(returnType, name);

                var methodParameters = VisitMethodParameterList(context.methodParameterList()) as List<ServiceMethodParameter>;
                foreach (var methodParameter in methodParameters)
                {
                    serviceMethod.AddMethodParameter(methodParameter);
                }

                return serviceMethod;
            }

            public override object VisitMethodParameterList([NotNull] SDLParser.MethodParameterListContext context)
            {
                var parentList = context.methodParameterList();
                var methodParameterList = (parentList != null) 
                    ? VisitMethodParameterList(parentList) as List<ServiceMethodParameter>
                    : new List<ServiceMethodParameter>();

                var methodParameter = VisitMethodParameter(context.methodParameter()) as ServiceMethodParameter;
                methodParameterList.Add(methodParameter);

                return methodParameterList;
            }

            public override object VisitMethodParameter([NotNull] SDLParser.MethodParameterContext context)
            {
                var type = VisitType(context.type()) as Type;
                var name = context.Identifier().GetText();
                var serviceMethodParameter = new ServiceMethodParameter(type, name);
                return serviceMethodParameter;
            }

            public override object VisitArrayType([NotNull] SDLParser.ArrayTypeContext context)
            {
                // TODO
                throw new System.NotImplementedException();
            }

            public override object VisitModelType([NotNull] SDLParser.ModelTypeContext context)
            {
                var name = context.Identifier().GetText();
                var model = _models.FirstOrDefault(m => m.Name == name)
                    ?? throw new CompilerException($"Use of undefined model '{name}'");
                return model;
            }

            public override object VisitInternalType([NotNull] SDLParser.InternalTypeContext context)
            {
                var name = context.GetText();

                // TODO: This could be refactored to use some metadata instead of switch-case
                switch (name)
                {
                    case "bool":
                        return new BoolType();
                    case "char":
                        return new CharType();
                    case "decimal":
                        return new DecimalType();
                    case "double":
                        return new DoubleType();
                    case "float":
                        return new FloatType();
                    case "int":
                        return new IntType();
                    case "long":
                        return new LongType();
                    case "short":
                        return new ShortType();
                    case "string":
                        return new StringType();
                    default:
                        throw new CompilerException($"Invalid internal type '{name}'");
                }
            }
        }
    }
}
