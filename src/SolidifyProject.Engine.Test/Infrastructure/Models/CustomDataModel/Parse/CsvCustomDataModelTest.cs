using System.Collections.Generic;
using NUnit.Framework;
using SolidifyProject.Engine.Infrastructure.Enums;

namespace SolidifyProject.Engine.Test.Infrastructure.Models.CustomDataModel.Parse
{
    [TestFixture]
    public class CsvCustomDataModelTest
    {
        [Test]
        public void ParseCsv()
        {
            var model = new Engine.Infrastructure.Models.CustomDataModel();
            model.Id = "file.csv";
            model.ContentRaw = 
                @"fName,    lName,    age
                  John,     Doe,      16
                  Mr,       Smith,    42";
            
            model.Parse();
            
            Assert.NotNull(model.DataType);
            Assert.AreEqual(CustomDataType.Csv.ToString(), model.DataType.ToString());
            
            var data = (List<dynamic>)model.CustomData;
            Assert.IsNotNull(data);
            Assert.AreEqual(2, data.Count);


            var item = data[0];
            Assert.AreEqual("John", item.fName);
            Assert.AreEqual("Doe", item.lName);
            Assert.AreEqual("16", item.age);
            
            item = data[1];
            Assert.AreEqual("Mr", item.fName);
            Assert.AreEqual("Smith", item.lName);
            Assert.AreEqual("42", item.age);
        }
    }
}