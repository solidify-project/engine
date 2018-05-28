using System;
using System.IO;
using System.Threading.Tasks;
using NUnit.Framework;
using SolidifyProject.Engine.Infrastructure.Interfaces;
using SolidifyProject.Engine.Infrastructure.Models;
using SolidifyProject.Engine.Services.ContentReaderService;
using SolidifyProject.Engine.Services.ContentWriterService;

namespace SolidifyProject.Engine.Test.Unit.Infrastructure.Services.ContentWriterService
{
    public class FileSystemContentWriterServiceTest : _ContentWriterServiceTestBase<CustomDataModel>
    {
        private static readonly string _root = Path.Combine(Path.GetTempPath(), "SolidifyProject.Test", DateTime.Now.ToString("yyyy-MM-dd hh-mm-ss"), "FileSystemContentWriterServiceTest");
        
        private readonly FileSystemTextContentReaderService<CustomDataModel> _binaryContentReaderService = new FileSystemTextContentReaderService<CustomDataModel>(_root);
        protected override IContentReaderService<CustomDataModel> ContentReaderService => _binaryContentReaderService;
        
        private readonly FileSystemTextContentWriterService<CustomDataModel> _contentWriterServiceBase = new FileSystemTextContentWriterService<CustomDataModel>(_root);
        protected override IContentWriterService<CustomDataModel> ContentWriterService => _contentWriterServiceBase;
        
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
        }

        [OneTimeTearDown]
        public void Cleanup()
        {
            if (Directory.Exists(_root))
            {
                Directory.Delete(_root, true);
            }
        }

        private static object[] _saveContentAsyncTestCases =
        {
            new object[] { new CustomDataModel { Id = "file.txt", ContentRaw = "la-la-la" } }
        };

        [TestCaseSource(nameof(_saveContentAsyncTestCases))]
        public override Task SaveContentAsyncTest(CustomDataModel newElement)
        {
            return base.SaveContentAsyncTest(newElement);
        }
        
        [TestCaseSource(nameof(_saveContentAsyncTestCases))]
        public override Task CleanOutputAsyncTest(CustomDataModel newElement)
        {
            return base.CleanOutputAsyncTest(newElement);
        }
    }
}