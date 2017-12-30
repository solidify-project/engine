using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NUnit.Framework;
using SolidifyProject.Engine.Infrastructure.Models;
using SolidifyProject.Engine.Infrastructure.Services;
using SolidifyProject.Engine.Test._Fake.Infrastructure.Services;

namespace SolidifyProject.Engine.Test.Infrastructure.Services.ContentReaderService
{
    public class ContentReaderServiceFakeTest : _ContentReaderServiceTestBase<CustomDataModel>
    {
        private readonly List<CustomDataModel> _storage = new List<CustomDataModel>
        {
            new CustomDataModel { Id = "1" },
            new CustomDataModel { Id = "2" },
            new CustomDataModel { Id = "3" },
            new CustomDataModel { Id = "README.md" }
        };
        protected override IContentReaderService<CustomDataModel> ContentReaderService => new ContentReaderServiceFake<CustomDataModel, string>(_storage);
        
        protected override bool AreEqual(CustomDataModel expected, CustomDataModel actual)
        {
            if (expected.Id.Equals(actual.Id, StringComparison.Ordinal)) return true;

            return false;
        }
        
        private static object[] _loadContentsIdsAsyncTestCases =
        {
            new object[]
            {
                new List<CustomDataModel>
                {
                    new CustomDataModel { Id = "1" },
                    new CustomDataModel { Id = "2" },
                    new CustomDataModel { Id = "3" }
                }
            }
        };
        
        [TestCaseSource(nameof(_loadContentsIdsAsyncTestCases))]
        public override Task LoadContentsIdsAsyncTest(IList<CustomDataModel> expected)
        {
            return base.LoadContentsIdsAsyncTest(expected);
        }

        
        private static object[] _loadContentByIdAsyncTestCases =
        {
            new object[] { "1", new CustomDataModel { Id = "1" } },
            new object[] { "2", new CustomDataModel { Id = "2" } },
            new object[] { "3", new CustomDataModel { Id = "3" } }
        };
        
        [TestCaseSource(nameof(_loadContentByIdAsyncTestCases))]
        public override Task LoadContentByIdAsyncTest(string contentId, CustomDataModel expected)
        {
            return base.LoadContentByIdAsyncTest(contentId, expected);
        }

        
        private static object[] _loadContentByNonExistingIdAsyncTestCases =
        {
            new object[] { "4" }
        };

        [TestCaseSource(nameof(_loadContentByNonExistingIdAsyncTestCases))]
        public override Task LoadContentByNonExistingIdAsyncTest(string contentId)
        {
            return base.LoadContentByNonExistingIdAsyncTest(contentId);
        }
    }
}