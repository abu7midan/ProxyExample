using DynamicProxy.Attributes;
using System;
using System.Linq;

namespace DynamicProxy.Extensions
{
    public static class TypeExtensions
    {
        public static bool IsExcluded(this Type type)
        {
            return type.GetCustomAttributes(true).OfType<ExcludeProxy>().Any();
        }

    }
}
