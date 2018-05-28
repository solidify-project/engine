using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NUnit.Framework;
using SolidifyProject.Engine.Infrastructure.Interfaces;
using SolidifyProject.Engine.Infrastructure.Models;
using SolidifyProject.Engine.Test.Unit._Fake.Infrastructure.Services;

namespace SolidifyProject.Engine.Test.Unit.Infrastructure.Services.ContentWriterService
{
    public class ContentWriteServiceFakeTest : _ContentWriterServiceTestBase<CustomDataModel>
    {
        private readonly List<CustomDataModel> _storage = new List<CustomDataModel>();
        
        protected override IContentReaderService<CustomDataModel> ContentReaderService => new ContentReaderServiceFake<CustomDataModel, string>(_storage);
        protected override IContentWriterService<CustomDataModel> ContentWriterService => new ContentWriterServiceFake<CustomDataModel>(_storage);
        
        protected override bool AreEqual(CustomDataModel expected, CustomDataModel actual)
        {
            if (expected.Id.Equals(actual.Id, StringComparison.Ordinal)) return true;

            return false;
        }
        
        private static object[] _saveContentAsyncTestCases =
        {
            new object[] { new CustomDataModel { Id = "4" } }
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