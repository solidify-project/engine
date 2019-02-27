using System.IO;
using SolidifyProject.Engine.Configuration.Models;
using SolidifyProject.Engine.Infrastructure.Models;
using SolidifyProject.Engine.Services.ContentReaderService;

namespace SolidifyProject.Engine.Configuration
{
    public sealed class ConfigurationService
    {
        public const string CONFIGURATION_FILE = "config.yaml";
        
        private SolidifyConfiguration _configuration; 
        private readonly string _root;
        
        public ConfigurationService(string root = null)
        {
            _root = string.IsNullOrEmpty(root) ? 
                Directory.GetCurrentDirectory() :
                root;
        }

        private SolidifyConfiguration GetConfiguration()
        {
            var reader = new FileSystemTextContentReaderService<CustomDataModel>(_root);
            var config = reader.LoadContentByIdAsync(CONFIGURATION_FILE).Result;
            
            if (config == null)
            {
                throw new FileNotFoundException("Solidify configuration file not found", Path.Combine(_root, CONFIGURATION_FILE));
            }

            var model = new SolidifyConfiguration(config);

            return model;
        }

        public SolidifyConfiguration Configuration => _configuration ?? (_configuration = GetConfiguration());
    }
}