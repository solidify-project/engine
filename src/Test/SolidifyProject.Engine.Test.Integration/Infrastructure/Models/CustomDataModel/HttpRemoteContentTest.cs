using System.Text;
using NUnit.Framework;

namespace SolidifyProject.Engine.Test.Integration.Infrastructure.Models.CustomDataModel
{
    [TestFixture]
    public class HttpRemoteContentTest
    {
        [Test]
        public void HttpRemoteYamlContentTest()
        {
            var model = new Engine.Infrastructure.Models.CustomDataModel();
            model.Id = "custom.http";

            var str = new StringBuilder();
            str.AppendLine("Url:             https://raw.githubusercontent.com/solidify-project/engine/master/_help/config.yaml");
            str.AppendLine("Method:          get");
            str.AppendLine("CustomDataType:  yaml");

            model.ContentRaw = str.ToString();
            
            model.Parse();

            Assert.AreEqual("engine", ((dynamic) model.CustomData).engine.path);
            Assert.AreEqual("src", ((dynamic) model.CustomData).source.path);
            Assert.AreEqual("www", ((dynamic) model.CustomData).output.path);
        }
    }
}