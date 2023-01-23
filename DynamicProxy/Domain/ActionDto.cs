using System.Collections.Generic;

namespace DynamicProxy.Domain
{
    public class ActionDto
    {
        public string Type { get; set; }

        public string Name { get; set; }

        public string Url { get; set; }

        public IEnumerable<ParameterDto> UrlParameters { get; set; }

        public ParameterDto BodyParameter { get; set; }

        public string Description { get; set; }

        public string ReturnType { get; set; }

    }
}
