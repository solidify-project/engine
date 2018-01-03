using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NUnit.Framework;
using SolidifyProject.Engine.Infrastructure.Models.Base;
using SolidifyProject.Engine.Infrastructure.Services;

namespace SolidifyProject.Engine.Test.Infrastructure.Services.ContentReaderService
{
    [TestFixture]
    public abstract class _ContentReaderServiceTestBase<T> where T : ModelBase
    {
        protected abstract IContentReaderService<T> ContentReaderService { get; }
        
        protected abstract bool AreEqual(T expected, T actual);
        
        [Test]
        public virtual async Task LoadContentsIdsAsyncExcludedTest(IList<T> expected)
        {
            var actual = (await ContentReaderService.LoadContentsIdsAsync()).ToList();
            
            Assert.AreEqual(expected.Count, actual.Count);
            foreach (var element in expected)
            {
                Assert.IsTrue(actual.Contains(element.Id), $"{element.Id} was not fount in actual collection");
            }
        }

        [Test]
        public virtual async Task LoadContentsIdsAsyncIncludedTest(IList<T> expected)
        {
            var actual = (await ContentReaderService.LoadContentsIdsAsync(true)).ToList();
            
            Assert.AreEqual(expected.Count, actual.Count);
            foreach (var element in expected)
            {
                Assert.IsTrue(actual.Contains(element.Id), $"{element.Id} was not fount in actual collection");
            }
        }
        
        [Test]
        public virtual async Task LoadContentByIdAsyncTest(string contentId, T expected)
        {
            var actual = await ContentReaderService.LoadContentByIdAsync(contentId);
            
            Assert.IsTrue(AreEqual(expected, actual), "Objects are not equal");
        }
        
        [Test]
        public virtual async Task LoadContentByNonExistingIdAsyncTest(string contentId)
        {
            var actual = await ContentReaderService.LoadContentByIdAsync(contentId);
            
            Assert.IsNull(actual);
        }
    }
}