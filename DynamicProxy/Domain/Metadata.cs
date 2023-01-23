using System.Collections.Generic;

namespace DynamicProxy.Domain
{
    public class Metadata
    {
        public string Host { get; set; }

        public IEnumerable<ControllerDto> Controllers { get; set; }

        public IEnumerable<ModelDto> Models { get; set; }

    }
}
