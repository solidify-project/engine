using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using NUnit.Framework;
using SolidifyProject.Engine.Infrastructure.Interfaces;
using SolidifyProject.Engine.Infrastructure.Models;
using SolidifyProject.Engine.Infrastructure.Models.Base;
using SolidifyProject.Engine.Infrastructure.Services;
using SolidifyProject.Engine.Services.ContentReaderService;
using SolidifyProject.Engine.Services.HtmlMinificationService;
using SolidifyProject.Engine.Services.MarkupService;
using SolidifyProject.Engine.Services.TemplateService;
using SolidifyProject.Engine.Test.Unit._Fake.Infrastructure.Services;

namespace SolidifyProject.Engine.Test.Integration.Infrastructure.Services
{
    [TestFixture]
    public class OrchestrationServiceTest
    {
        private static string _root = Path.Combine(Directory.GetCurrentDirectory(), "_TestData");
        
        private ILoggerService LoggerService = new LoggerServiceFake();
        
        private IContentReaderService<BinaryContentModel> AssetsReaderService => new FileSystemBinaryContentReaderService<BinaryContentModel>(Path.Combine(_root, "assets"));
        
        private readonly List<BinaryContentModel> _assetsWriterStorage = new List<BinaryContentModel>();
        private IContentWriterService<BinaryContentModel> AssetsWriterService => new ContentWriterServiceFake<BinaryContentModel>(_assetsWriterStorage);
        
        private IContentReaderService<PageModel> PageModelReaderService => new FileSystemTextContentReaderService<PageModel>(Path.Combine(_root, "pages"));
        
        private readonly List<TextContentModel> _pageModelWriterStorage = new List<TextContentModel>();
        private IContentWriterService<TextContentModel> PageModelWriterService => new ContentWriterServiceFake<TextContentModel>(_pageModelWriterStorage);
        
        private IContentReaderService<TemplateModel> TemplateReaderService => new FileSystemTextContentReaderService<TemplateModel>(Path.Combine(_root, "layout"));

        private ITemplateService TemplateService => new MustacheTemplateService(new FileSystemTextContentReaderService<TextContentModel>(Path.Combine(_root, "layout", "partials")));

        private IDataService DataService = new DataService(new FileSystemTextContentReaderService<CustomDataModel>(Path.Combine(_root, "data")));
        
        private IMarkupService MarkupService => new MarkdownMarkupService();

        private IHtmlMinificationService HtmlMinificationService => new GoogleHtmlMinificationService();

        [Test]
        public async Task ProduceHtml()
        {
            Assert.AreEqual(0, _pageModelWriterStorage.Count);
            Assert.AreEqual(0, _assetsWriterStorage.Count);
            
            var orchestrationService = new OrchestrationService();
            orchestrationService.LoggerService = LoggerService;
            orchestrationService.AssetsReaderService = AssetsReaderService;
            orchestrationService.AssetsWriterService = AssetsWriterService;
            orchestrationService.PageModelReaderService = PageModelReaderService;
            orchestrationService.PageModelWriterService = PageModelWriterService;
            orchestrationService.TemplateReaderService = TemplateReaderService;
            orchestrationService.TemplateService = TemplateService;
            orchestrationService.DataService = DataService;
            orchestrationService.MarkupService = MarkupService;
            orchestrationService.HtmlMinificationService = HtmlMinificationService;

            await orchestrationService.RenderProjectAsync();

            
            Assert.AreEqual(2, _pageModelWriterStorage.Count);
            Assert.IsFalse(_pageModelWriterStorage.All(x => x.ContentRaw == null));
            
            Assert.AreEqual(2, _assetsWriterStorage.Count);
            Assert.IsFalse(_assetsWriterStorage.All(x => x.ContentRaw == null));

            var logoAsset = _assetsWriterStorage.SingleOrDefault(x => x.Id.Contains("img") && x.Id.Contains("logo.png"));
            Assert.IsNotNull(logoAsset);

            var page = _pageModelWriterStorage.Single(x => x.Id.Equals("index.html"));
            
            Assert.AreEqual("<html><head><link href=\"/assets/style.css\" rel=\"stylesheet\" type=\"text/css\"><title>Static Site Generator - Home</title></head><body><div><ul><li><a href=\"https://facebook.com\">Facebook</a></li><li><a href=\"https://twitter.com\">Twitter</a></li></ul></div><h1>Welcome to Static Site Generator!</h1><h4>Authors</h4><h5>Founders</h5><ul><li>Anton Boyko (Microsoft Azure MVP)</li></ul><h5>Contributors</h5><p><strong>Powered by .NET Core 2.0</strong></p><div><i>All rights reserved</i></div></body></html>", page.ContentRaw);
        }
    }
}