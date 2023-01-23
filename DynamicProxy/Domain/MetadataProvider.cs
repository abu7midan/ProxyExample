using DynamicProxy.Extensions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.AspNetCore.Mvc.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Reflection;

namespace DynamicProxy.Domain
{
    public class MetadataProvider
    {
        private readonly List<ModelDto> models;
        private readonly List<string> typesToIgnore = new List<string>();
        private readonly IApiDescriptionGroupCollectionProvider _apiDescriptionsProvider;

        public MetadataProvider(IApiDescriptionGroupCollectionProvider apiDescriptionsProvider
)
        {

            this.models = new List<ModelDto>();
            this.typesToIgnore = new List<string>();
            _apiDescriptionsProvider = apiDescriptionsProvider;
        }

        public Metadata GetMetadata(HttpRequest request)
        {
            var host =  request.Host.Value;
            var descriptions = _apiDescriptionsProvider.ApiDescriptionGroups.Items.SelectMany(group => group.Items); 

            ILookup<ControllerActionDescriptor, ApiDescription> apiGroups = descriptions
                .Where(a => !(a.ActionDescriptor as ControllerActionDescriptor).ControllerTypeInfo.IsAbstract
                    && !a.RelativePath.Contains("Swagger")
                    && !a.RelativePath.Contains("docs"))
                .ToLookup(a => (a.ActionDescriptor as ControllerActionDescriptor));

            var metadata = new Metadata
            {
                Controllers = from d in apiGroups
                              where !d.Key.ControllerTypeInfo.IsExcluded()
                              select new ControllerDto
                              {
                                  Name = d.Key.ControllerName,
                                  ActionMethods = from a in descriptions
                                                  where !(a.ActionDescriptor as ControllerActionDescriptor).ControllerTypeInfo.IsExcluded()
                                                  && !(a.ActionDescriptor as ControllerActionDescriptor).MethodInfo.GetType().IsExcluded()
                                                  && !a.RelativePath.Contains("Swagger")
                                                  && !a.RelativePath.Contains("docs")
                                                  && (a.ActionDescriptor as ControllerActionDescriptor).ControllerName == d.Key.ControllerName
                                                  select new ActionDto
                                                  {
                                                      Name = (a.ActionDescriptor as ControllerActionDescriptor).ActionName,
                                                      BodyParameter = (from b in a.ParameterDescriptions
                                                                       where b.IsFromBody()
                                                                       select new ParameterDto
                                                                       {
                                                                           Name = b.ParameterDescriptor.Name,
                                                                           Type = ParseType(b.ParameterDescriptor.ParameterType)
                                                                       }).FirstOrDefault(),
                                                      UrlParameters = from b in a.ParameterDescriptions.Where(p => p.ParameterDescriptor != null)
                                                                      where b.IsFromPath()
                                                                      select new ParameterDto
                                                                      {
                                                                          Name = b.ParameterDescriptor.Name,
                                                                          Type = ParseType(b.ParameterDescriptor.ParameterType),
                                                                          IsOptional = b.IsRequiredParameter(),
                                                                          DefaultValue = b.DefaultValue
                                                                      },
                                                      Url = a.RelativePath,

                                                      ReturnType = ParseType(a.SupportedResponseTypes.FirstOrDefault(r=>r.StatusCode==(int)HttpStatusCode.OK).Type),
                                                      Type = a.HttpMethod
                                                  }
                              },
                Models = models,
                Host = (null != host && host.Length > 0 && host[host.Length - 1] != '/') ? string.Concat(host, "/") : host
            };

            metadata.Controllers = metadata.Controllers.Distinct().OrderBy(d => d.Name);
            metadata.Models = metadata.Models.Distinct(new ModelDtoEqualityComparer()).OrderBy(d => d.Name);
            return metadata;

        }

        private string ParseType(Type type, ModelDto model = null)
        {
            string res;

            if (type == null)
                return "";

            // If the type is a generic type format to correct class name.
            if (type.IsGenericType)
            {
                res = GetGenericRepresentation(type, (t) => ParseType(t, model), model);

                AddModelDtos(type);
            }
            else
            {
                if (type.ToString().StartsWith("System."))
                {
                    if (type.ToString().Equals("System.Void"))
                        res = "void";
                    else
                        res = type.Name;
                }
                else
                {
                    res = type.Name;

                    if (!type.IsGenericParameter)
                    {
                        AddModelDtos(type);
                    }
                }
            }

            return res;
        }

        private string GetGenericRepresentation(Type type, Func<Type, string> getTypedParameterRepresentation, ModelDto model = null)
        {
            string res = type.Name;
            int index = res.IndexOf('`');
            if (index > -1)
                res = res.Substring(0, index);

            Type[] args = type.GetGenericArguments();

            res += "<";

            for (int i = 0; i < args.Length; i++)
            {
                if (i > 0)
                    res += ", ";
                //Recursivly find nested arguments

                var arg = args[i];
                if (model != null && model.IsGenericArgument(arg.Name))
                {
                    res += model.GetGenericArgument(arg.Name);
                }
                else
                {
                    res += getTypedParameterRepresentation(arg);
                }
            }
            res += ">";
            return res;
        }

        private string GetGenericTypeDefineRepresentation(Type genericTypeDefClass)
        {

            string res = genericTypeDefClass.Name;
            int index = res.IndexOf('`');
            if (index > -1)
                res = res.Substring(0, index);

            Type[] args = genericTypeDefClass.GetGenericArguments();

            res += "<";

            for (int i = 0; i < args.Length; i++)
            {
                if (i > 0)
                    res += ", ";

                var arg = args[i];
                res += arg.Name;
            }

            res += ">";
            return res;
        }


        private void AddModelDtos(Type classToDef)
        {
            //When the class is an array redefine the classToDef as the array type
            if (classToDef.IsArray)
            {
                classToDef = classToDef.GetElementType();
            }
            // Is is not a .NET Framework generic, then add to the models collection.
            if (classToDef.Namespace.StartsWith("System", StringComparison.OrdinalIgnoreCase))
            {
                AddTypeToIgnore(classToDef.Name);
                return;
            }
            //If the class has not been mapped then map into metadata
            if (!typesToIgnore.Contains(classToDef.Name))
            {
                ModelDto model = new ModelDto();
                model.Name = classToDef.Name;
                model.Description = GetDescription(classToDef);
                if (classToDef.IsGenericType)
                {
                    model.Name = GetGenericTypeDefineRepresentation(classToDef.GetGenericTypeDefinition());
                }
                model.Type = classToDef.IsEnum ? "enum" : "class";
                var constants = classToDef
                    .GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy)
                    .Where(f => f.IsLiteral && !f.IsInitOnly)
                    .ToList();
                model.Constants = from constant in constants
                                  select new ConstantDto
                                  {
                                      Name = constant.Name,
                                      Type = ParseType(constant.FieldType),
                                      Value = GetConstantValue(constant),
                                      Description = GetDescription(constant)
                                  };

                var properties = classToDef.IsGenericType
                                     ? classToDef.GetGenericTypeDefinition().GetProperties()
                                     : classToDef.GetProperties();

                model.Properties = from property in properties
                                   select new PropertyDto
                                   {
                                       Name = property.Name,
                                       Type = ParseType(property.PropertyType, model),
                                       Description = GetDescription(property)
                                   };
                AddTypeToIgnore(model.Name);
                foreach (var p in properties)
                {
                    var type = p.PropertyType;

                    if (!models.Any(c => c.Name.Equals(type.Name)))// && !type.IsInterface)
                    {
                        ParseType(type);
                    }
                }
                models.Add(model);
            }
        }

        private void AddTypeToIgnore(string name)
        {
            if (!typesToIgnore.Contains(name))
            {
                typesToIgnore.Add(name);
            }
        }

        private string GetConstantValue(FieldInfo constant)
        {
            var value = constant.GetRawConstantValue().ToString();
            return value;
        }

        private static string GetDescription(MemberInfo member)
        {
            var xml = DocsService.GetXmlFromMember(member, false);
            if (xml != null)
            {
                return xml.InnerText.Trim();
            }
            return String.Empty;
        }

        private static string GetDescription(Type type)
        {
            var xml = DocsService.GetXmlFromType(type, false);

            if (xml != null)
            {
                return xml.InnerText.Trim();
            }
            return String.Empty;
        }
    }
}

