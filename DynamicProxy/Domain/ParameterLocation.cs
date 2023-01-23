using DynamicProxy.Exceptions;
using DynamicProxy.Extensions;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net.Http;
using System.Reflection;

namespace DynamicProxy.Domain
{
    public enum ParameterLocation
    {
        Query,
        Header,
        Path,
        Cookie
    }
}
