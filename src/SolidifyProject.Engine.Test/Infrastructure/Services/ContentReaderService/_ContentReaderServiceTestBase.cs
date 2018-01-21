using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NUnit.Framework;
using SolidifyProject.Engine.Infrastructure.Interfaces;
using SolidifyProject.Engine.Infrastructure.Models;

namespace SolidifyProject.Engine.Test.Infrastructure.Services.ContentReaderService
{
    [TestFixture]
    public abstract class _ContentReaderServiceTestBase<T> where T : CustomDataModel, new()
    {
        protected abstract IContentReaderService<T> ContentReaderService { get; }
        
        protected abstract bool AreEqual(T expected, T actual);

        protected abstract Task WriteContentAsync(T model);
        protected abstract Task CleanupAsync();

        [SetUp]
        public abstract Task Setup();
        
        [TearDown]
        public async Task Cleanup()
        {
            await CleanupAsync();
        }
        
        
        [Test]
        public async Task LoadContentsIdsExcluded()
        {
            await WriteContentAsync(new T { Id = "1.txt"});
            await WriteContentAsync(new T { Id = "2.txt"});
            await WriteContentAsync(new T { Id = "3.txt"});
            await WriteContentAsync(new T { Id = "README.md"});

            var expected = new List<T>
            {
                new T {Id = "1.txt"},
                new T {Id = "2.txt"},
                new T {Id = "3.txt"}
            };
            
            var actual = (await ContentReaderService.LoadContentsIdsAsync()).ToList();
            
            Assert.AreEqual(expected.Count, actual.Count);
            foreach (var element in expected)
            {
                Assert.IsTrue(actual.Contains(element.Id), $"{element.Id} was not fount in actual collection");
            }
        }
        
        [Test]
        public async Task LoadContentsIdsIncluded()
        {
            await WriteContentAsync(new T { Id = "1.txt"});
            await WriteContentAsync(new T { Id = "2.txt"});
            await WriteContentAsync(new T { Id = "3.txt"});
            await WriteContentAsync(new T { Id = "README.md"});

            var expected = new List<T>
            {
                new T {Id = "1.txt"},
                new T {Id = "2.txt"},
                new T {Id = "3.txt"},
                new T {Id = "README.md"}
            };
            
            var actual = (await ContentReaderService.LoadContentsIdsAsync(true)).ToList();
            
            Assert.AreEqual(expected.Count, actual.Count);
            foreach (var element in expected)
            {
                Assert.IsTrue(actual.Contains(element.Id), $"{element.Id} was not fount in actual collection");
            }
        }
        
        protected static object[] _loadContentByIdTestCases =
        {
            new object[] { "1.txt", new T { Id = "1.txt", ContentRaw = "some text 1" } },
            new object[] { "2.txt", new T { Id = "2.txt", ContentRaw = "some text 1"  } },
            new object[] { "3.txt", new T { Id = "3.txt", ContentRaw = "some text 1"  } }
        };
        
        [Test]
        [TestCaseSource(nameof(_loadContentByIdTestCases))]
        public async Task LoadContentById(string contentId, T expected)
        {
            await WriteContentAsync(expected);
            
            var actual = await ContentReaderService.LoadContentByIdAsync(contentId);
            
            Assert.IsTrue(AreEqual(expected, actual), "Objects are not equal");
        }
        
        
        protected static object[] _loadContentByNonExistingIdTestCases =
        {
            new object[] { "4.txt" },
            new object[] { "README.md" }
        };
        
        [Test]
        [TestCaseSource(nameof(_loadContentByNonExistingIdTestCases))]
        public async Task LoadContentByNonExistingId(string contentId)
        {
            var actual = await ContentReaderService.LoadContentByIdAsync(contentId);
            
            Assert.IsNull(actual);
        }
    }
}