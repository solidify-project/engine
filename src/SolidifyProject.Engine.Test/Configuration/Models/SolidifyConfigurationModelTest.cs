using NUnit.Framework;
using SolidifyProject.Engine.Configuration;
using SolidifyProject.Engine.Configuration.Models;
using SolidifyProject.Engine.Infrastructure.Models;

namespace SolidifyProject.Engine.Test.Configuration.Models
{
    [TestFixture]
    public class SolidifyConfigurationModelTest
    {
        private string _config = @"
            source:
              path: ./src
            
            output:
              path: ./www
        ";
        
        [Test]
        public void ConstructorTest()
        {
            var model = new CustomDataModel();
            model.Id = ConfigurationService.CONFIGURATION_FILE;
            model.ContentRaw = _config;
            model.Parse();

            var config = new SolidifyConfiguration(model);
            
            Assert.AreEqual("./src", config.Source.Path);
            Assert.AreEqual("./www", config.Output.Path);
        }
    }
}