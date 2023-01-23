using System.Collections.Generic;

namespace DynamicProxy.Domain
{
    public class ControllerDto
    {
        public string Name { get; set; }

        public string Description { get; set; }

        public IEnumerable<ActionDto> ActionMethods { get; set; }

    }
}
