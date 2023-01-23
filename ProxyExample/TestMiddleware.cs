using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Options;
using Swashbuckle.AspNetCore.SwaggerUI;
using System.Reflection;
using System.Text.Json.Serialization;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Text;
using DynamicProxy.Domain;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using DynamicProxy.Templates;

namespace ProxyExample
{
    public class TestMiddleware
    {

        private readonly StaticFileMiddleware _staticFileMiddleware;
        private readonly JsonSerializerOptions _jsonSerializerOptions;
        private MetadataProvider _metadataProvider;
        public TestMiddleware(
            RequestDelegate next,
            IWebHostEnvironment hostingEnv,
            ILoggerFactory loggerFactory,
            IApiDescriptionGroupCollectionProvider apiDescriptionsProvider
            )
        {

            _metadataProvider = new MetadataProvider(apiDescriptionsProvider);
            _jsonSerializerOptions = new JsonSerializerOptions();
#if NET6_0
            _jsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
#else
            _jsonSerializerOptions.IgnoreNullValues = true;
#endif
            _jsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
            _jsonSerializerOptions.Converters.Add(new JsonStringEnumConverter(JsonNamingPolicy.CamelCase, false));
        }

        public async Task Invoke(HttpContext httpContext)
        {
            var httpMethod = httpContext.Request.Method;
            var path = httpContext.Request.Path.Value;

            var metadata = _metadataProvider.GetMetadata(httpContext.Request);

            var template = new JavaScriptTemplate(metadata);
            StringContent js = new StringContent(template.TransformText());
            var jss = await js.ReadAsStringAsync();
        }

    }
}