using System.Threading.Tasks;
using NUnit.Framework;
using SolidifyProject.Engine.Infrastructure.Services;

namespace SolidifyProject.Engine.Test.Infrastructure.Services.MarkupService
{
    [TestFixture]
    public abstract class _MarkupServiceTestBase
    {
        protected abstract IMarkupService MarkupService { get; }

        [Test]
        public virtual async Task RenderMarkupTest(string template, string html)
        {
            var actualHtml = await MarkupService.RenderMarkupAsync(template);
            
            Assert.AreEqual(html, actualHtml);
        }
    }
}