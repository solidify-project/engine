using System;
using System.Threading.Tasks;
using NUnit.Framework;
using SolidifyProject.Engine.Infrastructure.Services;

namespace SolidifyProject.Engine.Test.Infrastructure.Services.HtmlCompressorService
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
        public async Task Html_Should_Be_NotNull()
        {
            Assert.ThrowsAsync<ArgumentNullException>(() => HtmlMinificationService.CompressHtmlAsync(null));
        }
    }
}