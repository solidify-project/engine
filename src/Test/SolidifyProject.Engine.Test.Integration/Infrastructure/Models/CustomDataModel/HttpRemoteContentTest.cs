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
        
        [Test]
        public void HttpRemoteTxtContentTest()
        {
            var model = new Engine.Infrastructure.Models.CustomDataModel();
            model.Id = "custom.http";

            var str = new StringBuilder();
            str.AppendLine("Url:             https://raw.githubusercontent.com/solidify-project/engine/master/_publish/win-x64/solidify.bat");
            str.AppendLine("Method:          get");
            str.AppendLine("CustomDataType:  txt");

            model.ContentRaw = str.ToString();
            
            model.Parse();

            Assert.AreEqual(@"engine\SolidifyProject.Engine.CLI.exe %*", ((dynamic) model.CustomData));
        }
        
        [Test]
        public void HttpRemoteJsonContentTest()
        {
            var model = new Engine.Infrastructure.Models.CustomDataModel();
            model.Id = "custom.http";

            var str = new StringBuilder();
            str.AppendLine("Url:             https://raw.githubusercontent.com/solidify-project/engine/master/src/SolidifyProject.Engine.CLI/Data/src/data/misc/Authors.json");
            str.AppendLine("Method:          get");
            str.AppendLine("CustomDataType:  json");

            model.ContentRaw = str.ToString();
            
            model.Parse();

            Assert.AreEqual(0, ((dynamic) model.CustomData).Contributors.Count);
            Assert.AreEqual(1, ((dynamic) model.CustomData).Founders.Count);
            Assert.AreEqual("Anton Boyko", (string)((dynamic) model.CustomData).Founders[0].Name);
            Assert.AreEqual("Microsoft Azure MVP", (string)((dynamic) model.CustomData).Founders[0].Title);
        }
    }
}