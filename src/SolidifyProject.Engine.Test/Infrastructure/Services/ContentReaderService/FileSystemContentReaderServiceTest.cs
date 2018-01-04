using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using NUnit.Framework;
using SolidifyProject.Engine.Infrastructure.Models;
using SolidifyProject.Engine.Infrastructure.Services;
using SolidifyProject.Engine.Services.ContentReaderService;
using SolidifyProject.Engine.Services.ContentWriterService;

namespace SolidifyProject.Engine.Test.Infrastructure.Services.ContentReaderService
{
    public class FileSystemContentReaderServiceTest : _ContentReaderServiceTestBase<CustomDataModel>
    {
        private static readonly string _root = Path.Combine(Path.GetTempPath(), "SSG.Test", DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss"), "FileSystemContentReaderServiceTest");
        
        private readonly FileSystemTextContentReaderService<CustomDataModel> _binaryContentReaderService = new FileSystemTextContentReaderService<CustomDataModel>(_root);
        protected override IContentReaderService<CustomDataModel> ContentReaderService => _binaryContentReaderService;
        
        private readonly FileSystemTextContentWriterService<CustomDataModel> _contentWriterServiceBase = new FileSystemTextContentWriterService<CustomDataModel>(_root);
        
        protected override bool AreEqual(CustomDataModel expected, CustomDataModel actual)
        {
            var equal = expected.Id.Equals(actual.Id, StringComparison.Ordinal)
                        && expected.ContentRaw.Equals(actual.ContentRaw);

            return equal;
        }
        
        [OneTimeSetUp]
        public void Init()
        {
            Directory.CreateDirectory(_root);
            foreach (object[] testCase in _loadContentByIdAsyncTestCases)
            {
                var model = testCase[1] as CustomDataModel;
                _contentWriterServiceBase.SaveContentAsync(model.Id, model).Wait();
            }
            
            _contentWriterServiceBase.SaveContentAsync("README.md", new CustomDataModel { Id = "README.md", ContentRaw = string.Empty }).Wait();
        }

        [OneTimeTearDown]
        public void Cleanup()
        {
            if (Directory.Exists(_root))
            {
                Directory.Delete(_root, true);
            }
        }
        
        
        private static object[] _loadContentsIdsAsyncExcludedTestCases =
        {
            new object[]
            {
                new List<CustomDataModel>
                {
                    new CustomDataModel { Id = "about.txt" },
                    new CustomDataModel { Id = "home.txt" }
                }
            }
        };
        
        [TestCaseSource(nameof(_loadContentsIdsAsyncExcludedTestCases))]
        public override Task LoadContentsIdsAsyncExcludedTest(IList<CustomDataModel> expected)
        {
            return base.LoadContentsIdsAsyncExcludedTest(expected);
        }
        
        
        private static object[] _loadContentsIdsAsyncIncludedTestCases =
        {
            new object[]
            {
                new List<CustomDataModel>
                {
                    new CustomDataModel { Id = "about.txt" },
                    new CustomDataModel { Id = "home.txt" },
                    new CustomDataModel { Id = "README.md" }
                }
            }
        };
        
        [TestCaseSource(nameof(_loadContentsIdsAsyncIncludedTestCases))]
        public override Task LoadContentsIdsAsyncIncludedTest(IList<CustomDataModel> expected)
        {
            return base.LoadContentsIdsAsyncIncludedTest(expected);
        }

        
        private static object[] _loadContentByIdAsyncTestCases =
        {
            new object[] { "about.txt", new CustomDataModel { Id = "about.txt", ContentRaw = "about content" } },
            new object[] { "home.txt", new CustomDataModel { Id = "home.txt", ContentRaw = "home content" } }
        };
        
        [TestCaseSource(nameof(_loadContentByIdAsyncTestCases))]
        public override Task LoadContentByIdAsyncTest(string contentId, CustomDataModel expected)
        {
            return base.LoadContentByIdAsyncTest(contentId, expected);
        }

        
        private static object[] _loadContentByNonExistingIdAsyncTestCases =
        {
            new object[] { "non-existing-file.txt" }
        };

        [TestCaseSource(nameof(_loadContentByNonExistingIdAsyncTestCases))]
        public override Task LoadContentByNonExistingIdAsyncTest(string contentId)
        {
            return base.LoadContentByNonExistingIdAsyncTest(contentId);
        }
    }
}