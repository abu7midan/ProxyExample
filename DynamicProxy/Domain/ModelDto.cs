using System.Collections.Generic;
using System.Diagnostics;

namespace DynamicProxy.Domain
{
    [DebuggerDisplay("{Name}")]
    public class ModelDto
    {
        #region Constructors
        public ModelDto()
        {
            GenericArgumentsMap = new Dictionary<string, string>();
            Constants = new List<ConstantDto>();
            Properties = new List<PropertyDto>();
        }
        #endregion

        #region Properties
        public string Name { get; set; }
        public string Description { get; set; }
        public string Type { get; set; }
        public IDictionary<string, string> GenericArgumentsMap { get; set; }

        public IEnumerable<ConstantDto> Constants { get; set; }
        public IEnumerable<PropertyDto> Properties { get; set; }
        #endregion

        #region Methods
        public string AddGenericArgument(string argument)
        {
            var result = System.String.Format(System.Globalization.CultureInfo.InvariantCulture, "T{0}", GenericArgumentsMap.Count + 1);
            GenericArgumentsMap.Add(argument, result);
            return result;
        }

        public string GetGenericArgument(string argument)
        {
            return GenericArgumentsMap[argument];
        }

        public bool IsGenericArgument(string argument)
        {
            return GenericArgumentsMap.ContainsKey(argument);
        }
        #endregion
    }
}
