using NUnit.Framework;
using SolidifyProject.Engine.Infrastructure.Enums;

namespace SolidifyProject.Engine.Test.Unit.Infrastructure.Models.CustomDataModel.Parse
{
    [TestFixture]
    public class JsonCustomDataModelTest
    {
        private string _json01 = @"
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
        public void ParseJson01()
        {
            var model = new Engine.Infrastructure.Models.CustomDataModel();
            model.Id = "file.json";
            model.ContentRaw = _json01;
            
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
        
        
        private string _json02 = @"
            { 
                ""peoples"": [
                    { 
                        ""fName""    :     ""John"", 
                        ""lName""    :     ""Doe"",
                        ""phones""   :     [
                            ""000"",
                            ""111""
                        ]
                    }
                ]
            }
        ";
        
        [Test]
        public void ParseJson02()
        {
            var model = new Engine.Infrastructure.Models.CustomDataModel();
            model.Id = "file.json";
            model.ContentRaw = _json02;
            
            model.Parse();
            
            Assert.NotNull(model.DataType);
            Assert.AreEqual(CustomDataType.Json.ToString(), model.DataType.ToString());
            
            var data = (dynamic)model.CustomData;
            Assert.IsNotNull(data);
            Assert.AreEqual(1, data.peoples.Count);
            
            Assert.AreEqual("John", (string)data.peoples[0].fName);
            Assert.AreEqual("Doe", (string)data.peoples[0].lName);
            
            Assert.AreEqual(2, data.peoples[0].phones.Count);
            Assert.AreEqual("000", (string)data.peoples[0].phones[0]);
            Assert.AreEqual("111", (string)data.peoples[0].phones[1]);
        }
    }
}