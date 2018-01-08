using NUnit.Framework;

namespace SolidifyProject.Engine.Test.Infrastructure.Models.CustomDataModel
{
    [TestFixture]
    public class CustomDataModelTest
    {
        private static object[] _sanitizedIdTestCases =
        {
            new object[] { @"4.txt",             "4" },
            new object[] { @"README.md",         "README" },
            new object[] { @"folder\file.txt",   "folder.file" },
            new object[] { @"p1/p2/ff.txt",      "p1.p2.ff" }
        };
        
        [Test]
        [TestCaseSource(nameof(_sanitizedIdTestCases))]
        public void SanitizedId(string id, string expected)
        {
            var obj = new Engine.Infrastructure.Models.CustomDataModel();
            obj.Id = id;
            
            Assert.AreEqual(expected, obj.SanitezedId);
        }
    }
}