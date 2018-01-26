using System.Collections.Generic;
using System.Dynamic;
using NUnit.Framework;
using SolidifyProject.Engine.Infrastructure.Services;

namespace SolidifyProject.Engine.Test.Infrastructure.Services.QueryService
{
    [TestFixture]
    public class CountTest
    {
        private static dynamic getDynamicCollection(params dynamic[] items)
        {
            var list = new List<dynamic>();

            if (items != null)
            {
                list.AddRange(items);
            }

            ICollection<KeyValuePair<string, object>> obj = new ExpandoObject();
            obj.Add(new KeyValuePair<string, object>(DataService.COLLECTION_PROPERTY, list));
            return obj;
        }

        private static readonly dynamic _dataEmpty = getDynamicCollection();
        private static readonly dynamic _dataSimple = getDynamicCollection(new {id = 1}, new {id = 2}, new {id = 3});
        
        public static object[] _countSimpleTestCases =
        {
            new object[] { _dataEmpty,  0, ""        },
            new object[] { _dataEmpty,  0, "id =  2" },
            new object[] { _dataEmpty,  0, "id != 2" },
            new object[] { _dataEmpty,  0, "id >  2" },
            new object[] { _dataEmpty,  0, "id >= 2" },
            new object[] { _dataEmpty,  0, "id <  2" },
            new object[] { _dataEmpty,  0, "id <= 2" },
            
            new object[] { _dataSimple, 3, ""        },
            new object[] { _dataSimple, 1, "id =  2" },
            new object[] { _dataSimple, 2, "id != 2" },
            new object[] { _dataSimple, 1, "id >  2" },
            new object[] { _dataSimple, 2, "id >= 2" },
            new object[] { _dataSimple, 1, "id <  2" },
            new object[] { _dataSimple, 2, "id <= 2" },
            
            new object[] { _dataSimple, 0, "id =  0" },
            new object[] { _dataSimple, 3, "id != 0" },
            new object[] { _dataSimple, 3, "id >  0" },
            new object[] { _dataSimple, 3, "id >= 0" },
            new object[] { _dataSimple, 0, "id <  0" },
            new object[] { _dataSimple, 0, "id <= 0" }
        };
        
        [Test]
        [TestCaseSource(nameof(_countSimpleTestCases))]
        public void CountSimpleTest(dynamic data, int expectedCount, string filter)
        {
            var service = new Engine.Infrastructure.Services.QueryService(data);

            var actualCount = service.Count(filter);
            
            Assert.AreEqual(expectedCount, actualCount);
        }
    }
}