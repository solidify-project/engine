using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using NUnit.Framework;
using SolidifyProject.Engine.Infrastructure.Services;

namespace SolidifyProject.Engine.Test.Infrastructure.Services.QueryService
{
    [TestFixture]
    public class OrderByTest
    {
        private static dynamic GetDynamicCollection(params dynamic[] items)
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

        private static readonly dynamic _dataEmpty = GetDynamicCollection();
        private static readonly dynamic _dataSimple = GetDynamicCollection(
            new {id = 1, name = "Bob",  age = 14},
            new {id = 2, name = "Paul", age = 42},
            new {id = 3, name = "Barb", age = 21},
            new {id = 4, name = "Bob",  age = 42});
        
        public static object[] _orderEmptyDataTestCases =
        {
            new object[] { _dataEmpty,   new int[]{},        ""                    },
            new object[] { _dataEmpty,   new int[]{},        "id"                  },
            new object[] { _dataEmpty,   new int[]{},        "name"                },
            new object[] { _dataEmpty,   new int[]{},        "age"                 },
            new object[] { _dataEmpty,   new int[]{},        "id asc"              },
            new object[] { _dataEmpty,   new int[]{},        "name desc"           },
            new object[] { _dataEmpty,   new int[]{},        "id, name"            },
            new object[] { _dataEmpty,   new int[]{},        "id desc, name asc"   }
        };
        
        [Test]
        [TestCaseSource(nameof(_orderEmptyDataTestCases))]
        public void OrderEmptyDataTest(dynamic data, int[] expectedOrder, string orderString)
        {
            var service = new Engine.Infrastructure.Services.QueryService(data);

            var actualOrder = service.Query(orderString: orderString).ToList();
            
            Assert.AreEqual(expectedOrder.Length, actualOrder.Count);
            for (var i = 0; i < expectedOrder.Length; i++)
            {
                Assert.AreEqual(expectedOrder[i], actualOrder[i].id);
            }
        }
        
        public static object[] _orderSimpleDataTestCases =
        {
            new object[] { _dataSimple,  new []{1, 2, 3, 4}, ""                    },
            new object[] { _dataSimple,  new []{1, 2, 3, 4}, "id"                  },
            new object[] { _dataSimple,  new []{3, 1, 4, 2}, "name"                },
            new object[] { _dataSimple,  new []{1, 3, 2, 4}, "age"                 },
            new object[] { _dataSimple,  new []{1, 2, 3, 4}, "id asc"              },
            new object[] { _dataSimple,  new []{2, 1, 4, 3}, "name desc"           },
            new object[] { _dataSimple,  new []{1, 2, 3, 4}, "id, name"            },
            new object[] { _dataSimple,  new []{4, 3, 2, 1}, "id desc, name asc"   },
            new object[] { _dataSimple,  new []{2, 4, 3, 1}, "age desc, name desc" }
        };
        
        [Test]
        [TestCaseSource(nameof(_orderSimpleDataTestCases))]
        public void OrderSimpleDataTest(dynamic data, int[] expectedOrder, string orderString)
        {
            var service = new Engine.Infrastructure.Services.QueryService(data);

            var actualOrder = service.Query(orderString: orderString).ToList();
            
            Assert.AreEqual(expectedOrder.Length, actualOrder.Count);
            for (var i = 0; i < expectedOrder.Length; i++)
            {
                Assert.AreEqual(expectedOrder[i], actualOrder[i].id);
            }
        }
    }
}