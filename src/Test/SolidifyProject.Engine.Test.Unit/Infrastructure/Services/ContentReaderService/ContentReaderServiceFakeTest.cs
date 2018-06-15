using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SolidifyProject.Engine.Infrastructure.Interfaces;
using SolidifyProject.Engine.Infrastructure.Models;
using SolidifyProject.Engine.Test.Unit._Fake.Infrastructure.Services;

namespace SolidifyProject.Engine.Test.Unit.Infrastructure.Services.ContentReaderService
{
    public class ContentReaderServiceFakeTest : _ContentReaderServiceTestBase<CustomDataModel>
    {
        private readonly List<CustomDataModel> _storage = new List<CustomDataModel>();
        protected override IContentReaderService<CustomDataModel> ContentReaderService => new ContentReaderServiceFake<CustomDataModel, string>(_storage);
        
        protected override bool AreEqual(CustomDataModel expected, CustomDataModel actual)
        {
            if (!expected.Id.Equals(actual.Id, StringComparison.Ordinal)) return false;

            return true;
        }

        protected override Task WriteContentAsync(CustomDataModel model)
        {
            _storage.Add(model);
            return Task.FromResult<object>(null);
        }

        public override Task Setup()
        {
            return Task.FromResult<object>(null);
        }
        
        protected override Task CleanupAsync()
        {
            _storage.Clear();
            return Task.FromResult<object>(null);
        }
    }
}