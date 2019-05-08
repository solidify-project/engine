using NUnit.Framework;
using SolidifyProject.Engine.Infrastructure.Enums;

namespace SolidifyProject.Engine.Test.Unit.Infrastructure.Models.CustomDataModel.Parse
{
    [TestFixture]
    public class JsonCustomDataModelTest
    {
        private string _jsonObject = @"
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
        public void ParseJsonObject()
        {
            var model = new Engine.Infrastructure.Models.CustomDataModel();
            model.Id = "file.json";
            model.ContentRaw = _jsonObject;
            
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
        
        
        private string _jsonArray = @"
            [
                { id: 1, name: ""John"" },
                { id: 2, name: ""Doe"" }
            ]
        ";
        
        [Test]
        public void ParseJsonArray()
        {
            var model = new Engine.Infrastructure.Models.CustomDataModel();
            model.Id = "file.json";
            model.ContentRaw = _jsonArray;
            
            model.Parse();
            
            Assert.NotNull(model.DataType);
            Assert.AreEqual(CustomDataType.Json.ToString(), model.DataType.ToString());
            
            var data = (dynamic)model.CustomData;
            Assert.IsNotNull(data);
            Assert.AreEqual(2, data.Count);
            
            Assert.AreEqual(1, (int)data[0].id);
            Assert.AreEqual("John", (string)data[0].name);
            
            Assert.AreEqual(2, (int)data[1].id);
            Assert.AreEqual("Doe", (string)data[1].name);
        }
    }
}