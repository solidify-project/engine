using NUnit.Framework;
using SolidifyProject.Engine.Infrastructure.Enums;

namespace SolidifyProject.Engine.Test.Unit.Infrastructure.Models.CustomDataModel.Parse
{
    [TestFixture]
    public class YamlCustomDataModelTest
    {
        private static object[] _fileNames =
        {
            new object[] { "file.yaml" },
            new object[] { "file.yml" },
        };
        
        [Test]
        [TestCaseSource(nameof(_fileNames))]
        public void ParseCollectionOfScalars(string fileName)
        {
            var model = new Engine.Infrastructure.Models.CustomDataModel();
            model.Id = fileName;
            model.ContentRaw = @"
                - aaa
                - bbb
            ";
            
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
        [TestCaseSource(nameof(_fileNames))]
        public void ParseCollectionOfStructures(string fileName)
        {
            var model = new Engine.Infrastructure.Models.CustomDataModel();
            model.Id = fileName;
            model.ContentRaw = @"
                -
                  name:     John
                  phones:
                    - iPhone
                                   
                -
                  name:     Bob
                  phones:
                    - Samsung
            ";
            
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
        [TestCaseSource(nameof(_fileNames))]
        public void ParseStructure(string fileName)
        {
            var model = new Engine.Infrastructure.Models.CustomDataModel();
            model.Id = fileName;
            model.ContentRaw = @" 
                name:    John
                phones:  
                  - iPhone
            ";
            
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
        [TestCaseSource(nameof(_fileNames))]
        public void ParseScalar(string fileName)
        {
            var model = new Engine.Infrastructure.Models.CustomDataModel();
            model.Id = fileName;
            model.ContentRaw = @"
                this
                is
                just
                text
            ";
            
            model.Parse();
            
            Assert.NotNull(model.DataType);
            Assert.AreEqual(CustomDataType.Yaml.ToString(), model.DataType.ToString());
            
            var data = (string)model.CustomData;
            Assert.IsNotNull(data);
            Assert.AreEqual("this is just text", data);
        }
    }
}