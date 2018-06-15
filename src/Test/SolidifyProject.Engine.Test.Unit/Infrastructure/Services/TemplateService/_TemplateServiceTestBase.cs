using System;
using System.Dynamic;
using System.Threading.Tasks;
using NUnit.Framework;
using SolidifyProject.Engine.Infrastructure.Interfaces;
using SolidifyProject.Engine.Infrastructure.Models;

namespace SolidifyProject.Engine.Test.Unit.Infrastructure.Services.TemplateService
{
    [TestFixture]
    public abstract class _TemplateServiceTestBase
    {
        protected abstract ITemplateService TemplateService { get; }

        [Test]
        public virtual async Task RenderTemplateTest(string template, PageModel pageModel, string html)
        {
            var actualHtml = await TemplateService.RenderTemplateAsync(template, pageModel, null);
            
            Assert.AreEqual(html, actualHtml);
        }

        [Test]
        public virtual async Task RenderTemplateWithNestetDataTest(string template, PageModel pageModel, ExpandoObject dataModel, string html)
        {
            var actualHtml = await TemplateService.RenderTemplateAsync(template, pageModel, dataModel);
            
            Assert.AreEqual(html, actualHtml);
        }

        [Test]
        public virtual async Task RenderTemplateWithLayoutTest(string template, PageModel pageModel, string html)
        {
            var actualHtml = await TemplateService.RenderTemplateAsync(template, pageModel, null);
            
            Assert.AreEqual(html, actualHtml);
        }

        [Test]
        public Task PageModel_Should_Be_NotNull()
        {
            PageModel pageModel = null;
            
            Assert.ThrowsAsync<ArgumentNullException>(async () => await RenderTemplateTest(null, pageModel, null));
            
            return Task.FromResult<object>(null);
        }
    }
}