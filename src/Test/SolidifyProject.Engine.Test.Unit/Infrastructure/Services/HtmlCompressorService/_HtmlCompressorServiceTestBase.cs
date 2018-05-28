using System;
using System.Threading.Tasks;
using NUnit.Framework;
using SolidifyProject.Engine.Infrastructure.Interfaces;

namespace SolidifyProject.Engine.Test.Unit.Infrastructure.Services.HtmlCompressorService
{
    [TestFixture]
    public abstract class _HtmlCompressorServiceTestBase
    {
        protected abstract IHtmlMinificationService HtmlMinificationService { get; }
        
        [Test]
        public virtual async Task CompressHtmlTest(string uncompressedHtml, string expectedHtml)
        {
            var actualHtml = await HtmlMinificationService.CompressHtmlAsync(uncompressedHtml);
            
            Assert.AreEqual(expectedHtml, actualHtml);
        }

        [Test]
        public Task Html_Should_Be_NotNull()
        {
            Assert.ThrowsAsync<ArgumentNullException>(() => HtmlMinificationService.CompressHtmlAsync(null));
            
            return Task.FromResult<object>(null);
        }
    }
}