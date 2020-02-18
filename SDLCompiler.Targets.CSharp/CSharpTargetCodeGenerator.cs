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
        public CSharpTargetOptions Options { get; }

        public CSharpTargetCodeGenerator()
        {
            Options = new CSharpTargetOptions()
            {
                UseAsyncMethods = true
            };
        }

        public void WriteModelCode(Model model, TextWriter writer)
        {
            writer.WriteLine($"public class {model.Name}");
            writer.WriteLine("{");

            foreach (var modelMember in model.Members)
            {
                var typeName = _GetTypeString(modelMember.Type);
                writer.WriteLine($"\tpublic {typeName} {modelMember.Name} {{ get; set; }}");
            }

            writer.WriteLine("}");
        }

        public void WriteInterfaceCode(Service service, TextWriter writer)
        {
            var methodRequestPayloadsStringBuilder = new StringBuilder();

            writer.WriteLine($"public interface I{service.Name}");
            writer.WriteLine("{");

            foreach (var serviceMethod in service.Methods)
            {
                var returnTypeName = _GetTypeString(serviceMethod.ReturnType);
                writer.Write("\t");
                if (Options.UseAsyncMethods)
                {
                    if (returnTypeName == "void")
                    {
                        writer.Write("Task");
                    }
                    else
                    {
                        writer.Write($"Task<{returnTypeName}>");
                    }
                }
                else
                {
                    writer.Write(returnTypeName);

                }
                writer.Write($" {serviceMethod.Name}");
                if (Options.UseAsyncMethods)
                {
                    writer.Write("Async");
                }
                writer.Write("(");

                methodRequestPayloadsStringBuilder.AppendLine($"public class {service.Name}_{serviceMethod.Name}_RequestDto");
                methodRequestPayloadsStringBuilder.AppendLine("{");

                bool writeComma = false;
                foreach (var methodParameter in serviceMethod.MethodParameters)
                {
                    if (writeComma)
                    {
                        writer.Write(", ");
                        writeComma = true;
                    }

                    var typeName = _GetTypeString(methodParameter.Type);
                    writer.Write($"{typeName} {methodParameter.Name}");

                    methodRequestPayloadsStringBuilder.AppendLine($"\tpublic {typeName} {methodParameter.Name} {{ get; set; }}");
                }

                writer.WriteLine(");");

                methodRequestPayloadsStringBuilder.AppendLine("}");
            }

            writer.WriteLine("}");

            writer.WriteLine(methodRequestPayloadsStringBuilder.ToString());
        }

        public void WriteEmptyServiceCode(Service service, TextWriter writer)
        {
            writer.WriteLine($"public class {service.Name} : I{service.Name}");
            writer.WriteLine("{");

            foreach (var serviceMethod in service.Methods)
            {
                var returnTypeName = _GetTypeString(serviceMethod.ReturnType);
                writer.Write("\tpublic ");
                if (Options.UseAsyncMethods)
                {
                    if (returnTypeName == "void")
                    {
                        writer.Write("Task");
                    }
                    else
                    {
                        writer.Write($"Task<{returnTypeName}>");
                    }
                }
                else
                {
                    writer.Write(returnTypeName);
                }
                writer.Write($" {serviceMethod.Name}");
                if (Options.UseAsyncMethods)
                {
                    writer.Write("Async");
                }
                writer.Write("(");

                bool writeComma = false;
                foreach (var methodParameter in serviceMethod.MethodParameters)
                {
                    if (writeComma)
                    {
                        writer.Write(", ");
                        writeComma = true;
                    }

                    var typeName = _GetTypeString(methodParameter.Type);
                    writer.Write($"{typeName} {methodParameter.Name}");
                }

                writer.WriteLine(")");
                writer.WriteLine("\t{");
                writer.WriteLine("\t\t// TODO");
                writer.WriteLine("\t\tthrow new NotImplementedException();");
                writer.WriteLine("\t}");
            }

            writer.WriteLine("}");
        }

        public void WriteClientServiceCode(Service service, TextWriter writer)
        {
            writer.WriteLine($"public class {service.Name}Client : I{service.Name}");
            writer.WriteLine("{");

            writer.WriteLine("\tpublic string ApiUrl { get; }");
            writer.WriteLine($"\tpublic {service.Name}Client(string apiUrl)");
            writer.WriteLine("\t{");
            writer.WriteLine("\t\tApiUrl = apiUrl;");
            writer.WriteLine("\t}");

            foreach (var serviceMethod in service.Methods)
            {
                var returnTypeName = _GetTypeString(serviceMethod.ReturnType);

                if (serviceMethod.ReturnType != null)
                {
                    writer.Write($"\tpublic async Task<{returnTypeName}> {serviceMethod.Name}Async(");
                }
                else
                {
                    writer.Write($"\tpublic async Task {serviceMethod.Name}Async(");
                }

                bool writeComma = false;
                foreach (var methodParameter in serviceMethod.MethodParameters)
                {
                    if (writeComma)
                    {
                        writer.Write(", ");
                        writeComma = true;
                    }

                    var typeName = _GetTypeString(methodParameter.Type);
                    writer.Write($"{typeName} {methodParameter.Name}");
                }

                writer.WriteLine(")");
                writer.WriteLine("\t{");
                writer.WriteLine("\t\tusing (var httpClient = new HttpClient())");
                writer.WriteLine("\t\t{");
                writer.WriteLine($"\t\t\tvar requestDto = new {service.Name}_{serviceMethod.Name}_RequestDto()");
                writer.WriteLine("\t\t\t{");

                foreach (var methodParameter in serviceMethod.MethodParameters)
                {
                    writer.WriteLine($"\t\t\t\t{methodParameter.Name} = {methodParameter.Name}");

                }

                writer.WriteLine("\t\t\t};");
                writer.WriteLine("\t\t\tvar json = JsonSerializer.Serialize(requestDto);");
                writer.WriteLine("\t\t\tvar content = new StringContent(json, Encoding.UTF8, \"application/json\");");
                writer.WriteLine($"\t\t\tvar response = await httpClient.PostAsync(ApiUrl + \"/api/{service.Name}/{serviceMethod.Name}\", content);");
                writer.WriteLine("\t\t\tstring jsonResponse = await response.Content.ReadAsStringAsync();");

                if (serviceMethod.ReturnType != null)
                {
                    writer.WriteLine($"\t\t\tvar result = JsonSerializer.Deserialize<{returnTypeName}>(jsonResponse, new JsonSerializerOptions() {{ PropertyNameCaseInsensitive = true }});");
                    writer.WriteLine("\t\t\treturn result;");
                }

                writer.WriteLine("\t\t}");
                writer.WriteLine("\t}");


                if (!Options.UseAsyncMethods)
                {
                    writer.Write($"\tpublic {returnTypeName} {serviceMethod.Name}(");

                    writeComma = false;
                    foreach (var methodParameter in serviceMethod.MethodParameters)
                    {
                        if (writeComma)
                        {
                            writer.Write(", ");
                            writeComma = true;
                        }

                        var typeName = _GetTypeString(methodParameter.Type);
                        writer.Write($"{typeName} {methodParameter.Name}");
                    }

                    writer.Write($") => {serviceMethod.Name}Async(");

                    writeComma = false;
                    foreach (var methodParameter in serviceMethod.MethodParameters)
                    {
                        if (writeComma)
                        {
                            writer.Write(", ");
                            writeComma = true;
                        }

                        writer.Write(methodParameter.Name);
                    }

                    writer.Write($").GetAwaiter().GetResult();");

                }
            }

            writer.WriteLine("}");
        }

        public void WriteServerServiceCode(Service service, TextWriter writer)
        {
            writer.WriteLine("[ApiController]");
            writer.WriteLine($"[Route(\"api/{service.Name}\")]");
            writer.WriteLine($"public class {service.Name}Controller : ControllerBase");
            writer.WriteLine("{");
            writer.WriteLine($"\tprivate readonly I{service.Name} {service.Name};");
            writer.WriteLine($"\tpublic {service.Name}Controller(I{service.Name} {service.Name})");
            writer.WriteLine("\t{");
            writer.WriteLine($"\t\tthis.{service.Name} = {service.Name};");
            writer.WriteLine("\t}");

            foreach (var serviceMethod in service.Methods)
            {
                var returnTypeName = _GetTypeString(serviceMethod.ReturnType);
              
                writer.WriteLine($"\t[HttpPost(\"{serviceMethod.Name}\")]");

                if (serviceMethod.ReturnType != null)
                {
                    writer.Write($"\tpublic async Task<ActionResult<{returnTypeName}>> {serviceMethod.Name}Async");
                }
                else
                {
                    writer.Write($"\tpublic async Task<ActionResult> {serviceMethod.Name}Async");
                }

                writer.WriteLine($"([FromBody] {service.Name}_{serviceMethod.Name}_RequestDto request)");


                writer.WriteLine("\t{");

                if (serviceMethod.ReturnType != null)
                {
                    writer.Write($"\t\tvar result = await this.{service.Name}.{serviceMethod.Name}Async(");
                }
                else
                {
                    writer.Write($"\t\tawait this.{service.Name}.{serviceMethod.Name}Async(");
                }

                bool writeComma = false;
                foreach (var methodParameter in serviceMethod.MethodParameters)
                {
                    if (writeComma)
                    {
                        writer.Write(", ");
                        writeComma = true;
                    }

                    writer.Write($"request.{methodParameter.Name}");
                }

                writer.WriteLine(");");

                if (serviceMethod.ReturnType != null)
                {
                    writer.WriteLine("\t\treturn Ok(result);");
                }
                else
                {
                    writer.WriteLine("\t\treturn Ok();");
                }

                writer.WriteLine("\t}");
            }

                

            writer.WriteLine("}");
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
