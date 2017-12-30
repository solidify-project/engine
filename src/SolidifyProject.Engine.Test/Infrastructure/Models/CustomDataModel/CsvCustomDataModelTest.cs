using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using SolidifyProject.Engine.Infrastructure.Enums;

namespace SolidifyProject.Engine.Test.Infrastructure.Models.CustomDataModel
{
    [TestFixture]
    public class CsvCustomDataModelTest
    {
        [Test]
        public void ParseTest()
        {
            var model = new Engine.Infrastructure.Models.CustomDataModel();
            model.Id = "file.csv";
            model.ContentRaw = 
                "fName,lName,age" + Environment.NewLine    // first line contains column names
                + "John,Doe,16" + Environment.NewLine
                + "Mr,Smith,42" + Environment.NewLine;
            
            model.Parse();
            
            Assert.NotNull(model.DataType);
            Assert.AreEqual(CustomDataType.Csv.ToString(), model.DataType.ToString());
            
            var data = (List<object>)model.CustomData;
            Assert.IsNotNull(data);
            Assert.AreEqual(2, data.Count);


            var item = (ICollection<KeyValuePair<string, object>>) data[0];
            Assert.AreEqual("John", item.Single(x => x.Key.Equals("fName")).Value);
            Assert.AreEqual("Doe", item.Single(x => x.Key.Equals("lName")).Value);
            Assert.AreEqual("16", item.Single(x => x.Key.Equals("age")).Value);
            
            item = (ICollection<KeyValuePair<string, object>>) data[1];
            Assert.AreEqual("Mr", item.Single(x => x.Key.Equals("fName")).Value);
            Assert.AreEqual("Smith", item.Single(x => x.Key.Equals("lName")).Value);
            Assert.AreEqual("42", item.Single(x => x.Key.Equals("age")).Value);
        }
    }
}