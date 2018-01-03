using SolidifyProject.Engine.Infrastructure.Models;

namespace SolidifyProject.Engine.Configuration.Models
{
    public sealed class SolidifyConfiguration
    {
        public SourceConfiguration Source { get; set; }
        public OutputConfiguration Output { get; set; }

        public SolidifyConfiguration(CustomDataModel model)
        {
            var config = model.CustomData as dynamic;
            
            Source = new SourceConfiguration();
            Source.Path = config.source.path;
            
            Output = new OutputConfiguration();
            Output.Path = config.output.path;
        }
    }
}