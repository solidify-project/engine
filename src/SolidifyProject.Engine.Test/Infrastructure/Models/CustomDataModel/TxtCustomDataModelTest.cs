using NUnit.Framework;
using SolidifyProject.Engine.Infrastructure.Enums;

namespace SolidifyProject.Engine.Test.Infrastructure.Models.CustomDataModel
{
    [TestFixture]
    public class TxtCustomDataModelTest
    {
        [Test]
        public void ParseTest()
        {
            var model = new Engine.Infrastructure.Models.CustomDataModel();
            model.Id = "file.txt";
            model.ContentRaw = "some text goes here";
            
            model.Parse();
            
            Assert.NotNull(model.DataType);
            Assert.AreEqual(CustomDataType.Txt.ToString(), model.DataType.ToString());
            
            var data = (string)model.CustomData;
            Assert.IsNotNull(data);
            Assert.AreEqual("some text goes here", data);
        }
    }
}