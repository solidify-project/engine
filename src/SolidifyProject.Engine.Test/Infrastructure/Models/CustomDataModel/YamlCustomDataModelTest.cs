using System;
using NUnit.Framework;
using SolidifyProject.Engine.Infrastructure.Enums;

namespace SolidifyProject.Engine.Test.Infrastructure.Models.CustomDataModel
{
    [TestFixture]
    public class YamlCustomDataModelTest
    {
        [Test]
        public void ParseCollectionOfScalarsTest()
        {
            var model = new Engine.Infrastructure.Models.CustomDataModel();
            model.Id = "file.yaml";
            model.ContentRaw = "- aaa" + Environment.NewLine +
                               "- bbb";
            
            model.Parse();
            
            Assert.NotNull(model.DataType);
            Assert.AreEqual(CustomDataType.Yaml.ToString(), model.DataType.ToString());
            
            var data = (dynamic)model.CustomData;
            Assert.IsNotNull(data);
            Assert.AreEqual(2, data.Count);
            Assert.AreEqual("aaa", data[0]);
            Assert.AreEqual("bbb", data[1]);
        }
        
        [Test]
        public void ParseCollectionOfStructuresTest()
        {
            var model = new Engine.Infrastructure.Models.CustomDataModel();
            model.Id = "file.yaml";
            model.ContentRaw = "-" + Environment.NewLine +
                               "  name: John" + Environment.NewLine +
                               "  phones:" + Environment.NewLine +
                               "    - iPhone" + Environment.NewLine +
                               
                               "-" + Environment.NewLine +
                               "  name: Bob" + Environment.NewLine +
                               "  phones:" + Environment.NewLine +
                               "    - Samsung";
            
            model.Parse();
            
            Assert.NotNull(model.DataType);
            Assert.AreEqual(CustomDataType.Yaml.ToString(), model.DataType.ToString());
            
            var data = (dynamic)model.CustomData;
            Assert.IsNotNull(data);
            Assert.AreEqual(2, data.Count);
            
            Assert.AreEqual("John", data[0].name);
            Assert.AreEqual(1, data[0].phones.Count);
            Assert.AreEqual("iPhone", data[0].phones[0]);
            
            Assert.AreEqual("Bob", data[1].name);
            Assert.AreEqual(1, data[1].phones.Count);
            Assert.AreEqual("Samsung", data[1].phones[0]);
        }
        
        [Test]
        public void ParseStructureTest()
        {
            var model = new Engine.Infrastructure.Models.CustomDataModel();
            model.Id = "file.yaml";
            model.ContentRaw = "name:    John" + Environment.NewLine +
                               "phones:  " + Environment.NewLine +
                               "  - iPhone" + Environment.NewLine;
            
            model.Parse();
            
            Assert.NotNull(model.DataType);
            Assert.AreEqual(CustomDataType.Yaml.ToString(), model.DataType.ToString());
            
            var data = (dynamic)model.CustomData;
            Assert.IsNotNull(data);
            
            Assert.AreEqual("John", data.name);
            Assert.AreEqual(1, data.phones.Count);
            Assert.AreEqual("iPhone", data.phones[0]);
        }
        
        [Test]
        public void ParseScalarTest()
        {
            var model = new Engine.Infrastructure.Models.CustomDataModel();
            model.Id = "file.yaml";
            model.ContentRaw = "this" + Environment.NewLine +
                               "is" + Environment.NewLine +
                               "just" + Environment.NewLine +
                               "text";
            
            model.Parse();
            
            Assert.NotNull(model.DataType);
            Assert.AreEqual(CustomDataType.Yaml.ToString(), model.DataType.ToString());
            
            var data = (string)model.CustomData;
            Assert.IsNotNull(data);
            Assert.AreEqual("this is just text", data);
        }
    }
}