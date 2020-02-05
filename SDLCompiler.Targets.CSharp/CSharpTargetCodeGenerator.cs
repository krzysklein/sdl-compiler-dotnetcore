using SDLCompiler.Domain;
using SDLCompiler.Domain.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace SDLCompiler.Targets.CSharp
{
    public class CSharpTargetCodeGenerator : ITargetCodeGenerator
    {
        public void WriteModelCode(Model model, TextWriter writer)
        {
            writer.WriteLine("public class {0}", model.Name);
            writer.WriteLine("{");

            foreach (var modelMember in model.Members)
            {
                var typeName = _GetTypeString(modelMember.Type);
                writer.WriteLine("\tpublic {0} {1} {{ get; set; }}", typeName, modelMember.Name);
            }

            writer.WriteLine("}");
        }


        public void WriteClientServiceCode(Service service, TextWriter writer)
        {
            throw new NotImplementedException();
        }



        public void WriteServerServiceCode(Service service, TextWriter writer)
        {
            throw new NotImplementedException();
        }

        private string _GetTypeString(Domain.Models.Type type)
        {
            if (type == null)
            {
                return "void";
            }
            else if (type.IsArrayType)
            {
                var arrayType = type as ArrayType;
                var baseType = _GetTypeString(arrayType.BaseType);
                var sb = new StringBuilder(baseType);
                for (int i = 0; i < arrayType.NumArrayDimensions; i++)
                {
                    sb.Append("[]");
                }
                return sb.ToString();
            }
            else if (type.IsInternalType)
            {
                var typeMappings = new Dictionary<string, string>
                {
                    ["bool"] = "bool",
                    ["char"] = "char",
                    ["decimal"] = "decimal",
                    ["double"] = "double",
                    ["float"] = "float",
                    ["int"] = "int",
                    ["long"] = "long",
                    ["short"] = "short",
                    ["string"] = "string"
                };

                if (!typeMappings.ContainsKey(type.Name))
                {
                    throw new Exception($"Use of unknown internal type {type.Name}");
                }
                return typeMappings[type.Name];
            }
            else if (type.IsModelType)
            {
                return type.Name;
            }
            else
            {
                throw new Exception($"Use of unsupported type {type.Name}");
            }
        }
    }
}
