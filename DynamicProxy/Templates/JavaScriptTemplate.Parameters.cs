using DynamicProxy.Domain;
namespace DynamicProxy.Templates
{
    public partial class JavaScriptTemplate
    {
        public JavaScriptTemplate(Metadata metadata)
        {
            this.Metadata = metadata??new Metadata();
        }
        public Metadata Metadata { get; set; }
    }
}
