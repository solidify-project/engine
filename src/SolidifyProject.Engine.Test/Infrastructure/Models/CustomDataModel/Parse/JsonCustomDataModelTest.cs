using NUnit.Framework;
using SolidifyProject.Engine.Infrastructure.Enums;

namespace SolidifyProject.Engine.Test.Infrastructure.Models.CustomDataModel.Parse
{
    [TestFixture]
    public class JsonCustomDataModelTest
    {
        private string _json = @"
            { 
                ""links"": [
                    { 
                        ""name""    :     ""facebook"", 
                        ""url""     :     ""https://facebook.com""
                    }, 
                    { 
                        ""name""    :     ""twitter"",
                        ""url""     :     ""https://twitter.com""
                    }
                ]
            }
        ";
        
        [Test]
        public void ParseJson()
        {
            var model = new Engine.Infrastructure.Models.CustomDataModel();
            model.Id = "file.json";
            model.ContentRaw = _json;
            
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