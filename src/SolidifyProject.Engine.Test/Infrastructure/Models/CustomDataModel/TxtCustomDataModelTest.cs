using NUnit.Framework;
using SolidifyProject.Engine.Infrastructure.Enums;

namespace SolidifyProject.Engine.Test.Infrastructure.Models.CustomDataModel
{
    [TestFixture]
    public class TxtCustomDataModelTest
    {
        private static object[] _parseTextTestCases =
        {
            new object[]
            {
                "some text goes here",
                "some text goes here"
            },
            new object[]
            {
                "      ",
                ""
            },
            new object[]
            {
                "      hello world       ",
                "hello world"
            }
        };
        
        [Test]
        [TestCaseSource(nameof(_parseTextTestCases))]
        public void ParseText(string raw, string expected)
        {
            var model = new Engine.Infrastructure.Models.CustomDataModel();
            model.Id = "file.txt";
            model.ContentRaw = raw;
            
            model.Parse();
            
            Assert.NotNull(model.DataType);
            Assert.AreEqual(CustomDataType.Txt.ToString(), model.DataType.ToString());
            
            var data = (string)model.CustomData;
            Assert.IsNotNull(data);
            Assert.AreEqual(expected, data);
        }
    }
}