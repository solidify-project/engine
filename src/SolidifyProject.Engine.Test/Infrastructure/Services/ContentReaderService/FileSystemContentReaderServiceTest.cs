using System;
using System.IO;
using System.Threading.Tasks;
using SolidifyProject.Engine.Infrastructure.Interfaces;
using SolidifyProject.Engine.Infrastructure.Models;
using SolidifyProject.Engine.Services.ContentReaderService;
using SolidifyProject.Engine.Services.ContentWriterService;

namespace SolidifyProject.Engine.Test.Infrastructure.Services.ContentReaderService
{
    public class FileSystemContentReaderServiceTest : _ContentReaderServiceTestBase<CustomDataModel>
    {
        private static readonly string _root = Path.Combine(Path.GetTempPath(), "SolidifyProject.Test", DateTime.Now.ToString("yyyy-MM-dd hh-mm-ss"), "FileSystemContentReaderServiceTest");
        
        private readonly FileSystemTextContentReaderService<CustomDataModel> _contentReaderService = new FileSystemTextContentReaderService<CustomDataModel>(_root);
        protected override IContentReaderService<CustomDataModel> ContentReaderService => _contentReaderService;
        
        private readonly FileSystemTextContentWriterService<CustomDataModel> _contentWriterServiceBase = new FileSystemTextContentWriterService<CustomDataModel>(_root);
        
        protected override bool AreEqual(CustomDataModel expected, CustomDataModel actual)
        {
            var equal = expected.Id.Equals(actual.Id, StringComparison.Ordinal)
                        && expected.ContentRaw.Equals(actual.ContentRaw);

            return equal;
        }

        protected override async Task WriteContentAsync(CustomDataModel model)
        {
            await _contentWriterServiceBase.SaveContentAsync(model.Id, model);
        }

        public override Task Setup()
        {
            Directory.CreateDirectory(_root);
            return Task.FromResult<object>(null);
        }
        
        protected override Task CleanupAsync()
        {
            Directory.Delete(_root, true);
            return Task.FromResult<object>(null);
        }
    }
}