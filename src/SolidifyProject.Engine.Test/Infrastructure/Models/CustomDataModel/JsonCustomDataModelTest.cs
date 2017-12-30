using NUnit.Framework;
using SolidifyProject.Engine.Infrastructure.Enums;

namespace SolidifyProject.Engine.Test.Infrastructure.Models.CustomDataModel
{
    [TestFixture]
    public class JsonCustomDataModelTest
    {
        [Test]
        public void ParseTest()
        {
            var model = new Engine.Infrastructure.Models.CustomDataModel();
            model.Id = "file.json";
            model.ContentRaw = "{ \"links\": [ { \"name\": \"facebook\", \"url\": \"https://facebook.com\" }, { \"name\": \"twitter\", \"url\": \"https://twitter.com\" } ] }";
            
            model.Parse();
            
            Assert.NotNull(model.DataType);
            Assert.AreEqual(CustomDataType.Json.ToString(), model.DataType.ToString());
            
            var data = (dynamic)model.CustomData;
            Assert.IsNotNull(data);
            Assert.AreEqual(2, data.links.Count);
            
            Assert.AreEqual("facebook", (string)data.links[0].name);
            Assert.AreEqual("https://facebook.com", (string)data.links[0].url);
            
            Assert.AreEqual("twitter", (string)data.links[1].name);
            Assert.AreEqual("https://twitter.com", (string)data.links[1].url);
        }
    }
}