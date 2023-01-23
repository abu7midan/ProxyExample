using DynamicProxy.Domain;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Collections.Generic;

namespace DynamicProxy.Extensions
{
    public class Constants
    {
        public static readonly Dictionary<string, HttpMethodType> HttpMethodTypeMap = new Dictionary<string, HttpMethodType>
        {
            { "GET", HttpMethodType.Get },
            { "PUT", HttpMethodType.Put },
            { "POST", HttpMethodType.Post },
            { "DELETE", HttpMethodType.Delete },
            { "OPTIONS", HttpMethodType.Options },
            { "HEAD", HttpMethodType.Head },
            { "PATCH", HttpMethodType.Patch },
            { "TRACE", HttpMethodType.Trace }
        };

        public static readonly Dictionary<BindingSource, ParameterLocation> ParameterLocationMap = new Dictionary<BindingSource, ParameterLocation>
        {
            { BindingSource.Query, ParameterLocation.Query },
            { BindingSource.Header, ParameterLocation.Header },
            { BindingSource.Path, ParameterLocation.Path }
        };

        public static readonly Dictionary<string, string> ResponseDescriptionMap = new Dictionary<string, string>
        {
            { "1\\d{2}", "Information" },
            { "2\\d{2}", "Success" },
            { "304", "Not Modified" },
            { "3\\d{2}", "Redirect" },
            { "400", "Bad Request" },
            { "401", "Unauthorized" },
            { "403", "Forbidden" },
            { "404", "Not Found" },
            { "405", "Method Not Allowed" },
            { "406", "Not Acceptable" },
            { "408", "Request Timeout" },
            { "409", "Conflict" },
            { "4\\d{2}", "Client Error" },
            { "5\\d{2}", "Server Error" },
            { "default", "Error" }
        };
    }
}
