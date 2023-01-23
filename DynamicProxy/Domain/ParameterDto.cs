namespace DynamicProxy.Domain
{
    public class ParameterDto
    {
        public string Name { get; set; }

        public string Type { get; set; }

        public string Description { get; set; }

        public bool IsOptional { get; set; }

        public object DefaultValue { get; set; }
        public ParameterLocation Location { get; set; }
        
    }
}
