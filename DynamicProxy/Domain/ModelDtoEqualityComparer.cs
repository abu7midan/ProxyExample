using System;
using System.Collections.Generic;

namespace DynamicProxy.Domain
{
    public class ModelDtoEqualityComparer : IEqualityComparer<ModelDto>
    {
        public bool Equals(ModelDto x, ModelDto y)
        {
            return x.Name.Equals(y.Name, StringComparison.OrdinalIgnoreCase);
        }

        public int GetHashCode(ModelDto obj)
        {
            return obj.Name.GetHashCode();
        }
    }
}
